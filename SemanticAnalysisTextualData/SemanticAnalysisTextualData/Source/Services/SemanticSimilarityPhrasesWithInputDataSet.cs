using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using OpenAI.Embeddings;
using SemanticAnalysisTextualData.Source.Interfaces;
using SemanticAnalysisTextualData.Source.pojo;
using SemanticAnalysisTextualData.Util;
using System.Globalization;

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
    public async Task InvokeProcessPhrases()
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
        string? projectRoot = AppContext.BaseDirectory;
        if (projectRoot == null)
        {
            Console.WriteLine("Project root is null.");
            return null;
        }
        var allRecords = new List<PhraseSimilarity>();
        var files = Directory.GetFiles(Path.Combine(projectRoot, "data"), "*_phrase_pairs.csv");
        //string datasetPath = files.Any() ? files[0] : string.Empty;

        //Added Code to Read CSV File Input for Phrase to Ease the User Convenience for Providing Additional Data for Analysis
        foreach (var file in files)
        {
            try
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<PhraseSimilarityInputCSV>()
                       .Select(result => new PhraseSimilarity
                       {
                           Context = result.Context,
                           Phrase1 = result.Phrase1,
                           Phrase2 = result.Phrase2,
                           Domain = result.Domain
                       })
                        .ToList();
                    allRecords.AddRange(records);
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dataset: {ex.Message}");
            }
        }
        return new InputDataset { PhrasePairs = allRecords };
    }

    /// <summary>
    /// Method is to Fetch the Project Root Directory, Currently not in use, if we needed in future we can use it
    /// </summary>
    /// <returns>The loaded dataset or null if an error occurs.</returns>
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
    /// Processes a list of phrase pairs to compute their semantic similarity.
    /// </summary>
    /// <param name="phrasePairs">The list of phrase pairs to process.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of phrase similarities.</returns>
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

    /// <summary>
    /// Creates a PhraseSimilarity object with the given similarity score.
    /// </summary>
    /// <param name="pair">The PhraseSimilarity object containing the phrases and their context.</param>
    /// <param name="similarity">The calculated similarity score between the two phrases.</param>
    /// <returns>A new PhraseSimilarity object with the similarity score included.</returns>
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
