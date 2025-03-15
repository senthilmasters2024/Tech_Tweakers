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
class SemanticSimilarityPhrasesWithInputDataSet : ISimilarityService
{
    private readonly EmbeddingClient _client;

    /// <summary>
    /// Constructor initializes the OpenAI Embedding Client.
    /// </summary>
    public SemanticSimilarityPhrasesWithInputDataSet()
    {
        _client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

    /// <summary>
    /// Main method to invoke processing of phrases for similarity analysis.
    /// Loads dataset, processes phrase pairs, and saves results.
    /// </summary>
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
    /// Loads the input dataset from a JSON file.
    /// </summary>
    /// <returns>Returns the dataset if successfully loaded; otherwise, null.</returns>
    private InputDataset? LoadDataset()
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

    /// <summary>
    /// Retrieves the root directory of the project.
    /// </summary>
    /// <returns>Returns the project root directory path.</returns>
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

    /// <summary>
    /// Processes a list of phrase pairs asynchronously and calculates similarity scores.
    /// </summary>
    /// <param name="phrasePairs">List of phrase pairs for similarity calculation.</param>
    /// <returns>Returns a list of phrase similarity results.</returns>
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
    /// Calculates the semantic similarity between two phrases using OpenAI embeddings.
    /// </summary>
    /// <param name="phrase1">First phrase.</param>
    /// <param name="phrase2">Second phrase.</param>
    /// <returns>Returns the similarity score between the two phrases.</returns>
    private async Task<double> CalculatePhraseSimilarityAsync(string phrase1, string phrase2)
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

    /// <summary>
    /// Creates a PhraseSimilarity object with the calculated similarity score.
    /// </summary>
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
    /// Saves the similarity results to JSON and CSV files.
    /// </summary>
    private void SaveResults(List<PhraseSimilarity> results)
    {
        string currentDir = Directory.GetCurrentDirectory();
        string outputPathJson = Path.Combine(currentDir, "data", "output_dataset.json");
        string outputPathCsv = Path.Combine(currentDir, "data", "output_datasetphrases.csv");

        var obj = new InputDataset { PhrasePairs = results };

        File.WriteAllText(outputPathJson, JsonConvert.SerializeObject(obj, Formatting.Indented));
        Console.WriteLine("Results saved to output_dataset.json.");

        using (var writer = new StreamWriter(outputPathCsv))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(results);
        }
        Console.WriteLine($"Phrase Similarity Results saved to {outputPathCsv}.");
    }

    /// <summary>
    /// Calculates the cosine similarity between two embedding vectors.
    /// </summary>
    /// <param name="embedding1">First embedding vector.</param>
    /// <param name="embedding2">Second embedding vector.</param>
    /// <returns>Returns a similarity score between 0 and 1.</returns>
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
    /// Not implemented: Asynchronous similarity calculation for text.
    /// </summary>
    public Task<double> CalculateSimilarityAsync(string text1, string text2)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Represents the dataset containing phrase pairs.
    /// </summary>
    class InputDataset
    {
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new List<PhraseSimilarity>();
    }
}
