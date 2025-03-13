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

class SemanticSimilarityPhrasesWithInputDataSet : ISimilarityService
{
    private readonly EmbeddingClient _client;

    public SemanticSimilarityPhrasesWithInputDataSet()
    {
        _client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

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
            Console.WriteLine($"Error in invokeProcessPhrases: {ex.Message}");
        }
    }

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
            var dataset = JsonConvert.DeserializeObject<InputDataset>(File.ReadAllText(datasetPath));
            return dataset;
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

    private async Task <List<PhraseSimilarity>>ProcessPhrasePairsAsync(List<PhraseSimilarity> phrasePairs)
    {
        var results = new List<PhraseSimilarity>();

        foreach (var pair in phrasePairs)
        {
            try
            {
                if (pair.Phrase1 != null && pair.Phrase2 != null)
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

    public Task<double> CalculateSimilarityAsync(string text1, string text2)
    {
        throw new NotImplementedException();
    }

    class InputDataset
    {
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new List<PhraseSimilarity>();
    }


}
