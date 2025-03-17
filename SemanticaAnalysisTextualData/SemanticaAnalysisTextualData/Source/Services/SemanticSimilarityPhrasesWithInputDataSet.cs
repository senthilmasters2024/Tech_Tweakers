using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.Interfaces;
using SemanticaAnalysisTextualData.Source.pojo;
using SemanticaAnalysisTextualData.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Class for computing semantic similarity between phrases using OpenAI embeddings.
/// Implements the ISimilarityService interface.
/// </summary>
public class SemanticSimilarityPhrasesWithInputDataSet : ISimilarityService
{
    private readonly EmbeddingClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticSimilarityPhrasesWithInputDataSet"/> class.
    /// </summary>
    public SemanticSimilarityPhrasesWithInputDataSet()
    {
        _client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

    /// <summary>
    /// Invokes the process to compute semantic similarity between phrases.
    /// </summary>
    /// <param name="args">The arguments for the process.</param>
    public async Task InvokeProcessPhrases(string[] args)
    {
        try
        {
            var dataset = LoadDataset();
            if (dataset == null)
            {
                Console.WriteLine("Dataset is null.");
                return;
            }

            var results = await ProcessPhrasePairsAsync(dataset.PhrasePairs);
            CsvHelperUtil.SaveResultsPhrase(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in InvokeProcessPhrases: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads the dataset from a JSON file.
    /// </summary>
    /// <returns>The loaded dataset or null if an error occurs.</returns>
    public InputDataset? LoadDataset()
    {
        string? projectRoot = GetProjectRoot();
        if (projectRoot == null)
        {
            Console.WriteLine("Project root is null.");
            return null;
        }

        string datasetPath = Path.Combine(projectRoot, "data", "InputPhrases50DataSet.json");

        try
        {
            return JsonConvert.DeserializeObject<InputDataset>(File.ReadAllText(datasetPath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading dataset: {ex.Message}");
            return null;
        }
    }

    private string? GetProjectRoot()
    {
        DirectoryInfo? directory = Directory.GetParent(AppContext.BaseDirectory);
        for (int i = 0; i < 3; i++)
        {
            if (directory == null)
            {
                return null;
            }
            directory = directory.Parent;
        }
        return directory?.FullName;
    }

    private async Task<List<PhraseSimilarity>> ProcessPhrasePairsAsync(List<PhraseSimilarity> phrasePairs)
    {
        var results = new List<PhraseSimilarity>();

        foreach (var pair in phrasePairs)
        {
            try
            {
                if (!string.IsNullOrEmpty(pair.Phrase1) && !string.IsNullOrEmpty(pair.Phrase2))
                {
                    var similarity = await CalculatePhraseSimilarityAsync(pair.Phrase1, pair.Phrase2);
                    var phraseSimilarity = CreatePhraseSimilarity(pair, similarity);
                    results.Add(phraseSimilarity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing phrase pair ({pair.Phrase1}, {pair.Phrase2}): {ex.Message}");
            }
        }

        return results;
    }

    /// <summary>
    /// Asynchronously calculates the similarity between two phrases using OpenAI embeddings.
    /// </summary>
    /// <param name="phrase1">The first phrase.</param>
    /// <param name="phrase2">The second phrase.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score.</returns>
    public async Task<double> CalculatePhraseSimilarityAsync(string phrase1, string phrase2)
    {
        try
        {
            List<string> inputs = new() { phrase1, phrase2 };
            OpenAIEmbeddingCollection collection = await _client.GenerateEmbeddingsAsync(inputs);

            float[] embedding1 = collection[0].ToFloats().ToArray();
            float[] embedding2 = collection[1].ToFloats().ToArray();

            return CalculateSimilarity(embedding1, embedding2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating similarity: {ex.Message}");
            return 0;
        }
    }

    private PhraseSimilarity CreatePhraseSimilarity(PhraseSimilarity pair, double similarity)
    {
        return new PhraseSimilarity
        {
            Phrase1 = pair.Phrase1,
            Phrase2 = pair.Phrase2,
            Domain = pair.Domain,
            Context = pair.Context,
            SimilarityScore = similarity
        };
    }

    /// <summary>
    /// Calculates the cosine similarity between two embedding vectors.
    /// </summary>
    /// <param name="embedding1">The first embedding vector.</param>
    /// <param name="embedding2">The second embedding vector.</param>
    /// <returns>The cosine similarity score between the two embedding vectors.</returns>
    public double CalculateSimilarity(float[] embedding1, float[] embedding2)
    {
        try
        {
            if (embedding1.Length != embedding2.Length)
            {
                return 0;
            }

            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                magnitude1 += Math.Pow(embedding1[i], 2);
                magnitude2 += Math.Pow(embedding2[i], 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            if (magnitude1 == 0.0 || magnitude2 == 0.0)
            {
                throw new ArgumentException("Embedding vectors must not have zero magnitude.");
            }

            return dotProduct / (magnitude1 * magnitude2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating cosine similarity: {ex.Message}");
            return 0;
        }
    }

    /// <summary>
    /// Asynchronously calculates the similarity between two texts.
    /// </summary>
    /// <param name="text1">The first text.</param>
    /// <param name="text2">The second text.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score.</returns>
    public Task<double> CalculateSimilarityAsync(string text1, string text2)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Represents the input dataset containing a list of phrase pairs.
    /// </summary>
    public class InputDataset
    {
        /// <summary>
        /// Gets or sets the list of phrase pairs.
        /// </summary>
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new();
    }
}
