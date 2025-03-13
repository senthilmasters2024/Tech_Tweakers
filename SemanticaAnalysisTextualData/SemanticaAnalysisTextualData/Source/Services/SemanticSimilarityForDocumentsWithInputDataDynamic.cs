using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class SemanticSimilarityForDocumentsWithInputDataDynamic : ISimilarityService, IEmbedding
{
    public async Task InvokeDocumentComparsion(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var textAnalysisService = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();

        if (textAnalysisService != null)
        {
            try
            {
                var (sourceFiles, targetFiles) = GetSourceAndTargetFiles();
                var results = await CompareDocumentsAsync(sourceFiles, targetFiles);
                SaveResultsToCsv(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SemanticSimilarityForDocumentsWithInputDataDynamic>(provider => new SemanticSimilarityForDocumentsWithInputDataDynamic());
        return services.BuildServiceProvider();
    }

    private (string[] sourceFiles, string[] targetFiles) GetSourceAndTargetFiles()
    {
        string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        string baseDataFolder = Path.Combine(projectRoot, "data");
        string sourceFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnDomains");
        string targetFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnNeededRelevance");

        var sourceFiles = Directory.GetFiles(sourceFolder, "*.txt");
        var targetFiles = Directory.GetFiles(targetFolder, "*.txt");

        return (sourceFiles, targetFiles);
    }

    private async Task<List<PhraseSimilarity>> CompareDocumentsAsync(string[] sourceFiles, string[] targetFiles)
    {
        var results = new List<PhraseSimilarity>();
        foreach (var sourceFile in sourceFiles)
        {
            string fileName1 = Path.GetFileName(sourceFile);
            Console.WriteLine($"File Name: {fileName1}");

            string sentence1 = await File.ReadAllTextAsync(sourceFile);

            foreach (var targetFile in targetFiles)
            {
                string fileName2 = Path.GetFileName(targetFile);
                Console.WriteLine($"File Name: {fileName2}");
                string sentence2 = await File.ReadAllTextAsync(targetFile);

                var similarity = await CalculateSimilarityAsync(sentence1, sentence2, fileName1, fileName2);
                Console.WriteLine($"Similarity between {fileName1} and {fileName2}: {similarity:F4}");

                var phraseSimilarity = CreatePhraseSimilarity(fileName1, fileName2, similarity);
                Console.WriteLine($"File: {sourceFile}, Domain: {phraseSimilarity.domain}");
                results.Add(phraseSimilarity);
            }
        }
        return results;
    }

    private PhraseSimilarity CreatePhraseSimilarity(string fileName1, string fileName2, double similarity)
    {
        var phraseSimilarity = new PhraseSimilarity
        {
            FileName1 = fileName1,
            FileName2 = fileName2,
            SimilarityScore = similarity
        };

        if (fileName1.StartsWith("preprocessed_JobProfile", StringComparison.OrdinalIgnoreCase))
        {
            phraseSimilarity.domain = "jobvacancy";
        }
        else if (fileName1.StartsWith("preprocessed_MedicalHistory", StringComparison.OrdinalIgnoreCase))
        {
            phraseSimilarity.domain = "Medical-MedicationSuggestion";
        }
        else
        {
            phraseSimilarity.domain = "Unknown"; // Default or fallback domain
        }

        return phraseSimilarity;
    }

    private void SaveResultsToCsv(List<PhraseSimilarity> results)
    {
        string currentDir = Directory.GetCurrentDirectory();
        string outputPath = Path.Combine(currentDir, "data", "output_dataset.csv");
        using (var writer = new StreamWriter(outputPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(results);
        }
        Console.WriteLine($"Results saved to {outputPath}.");
    }

    public async Task<double> CalculateSimilarityAsync(string text1, string text2, string fileName1, string fileName2)
    {
        try
        {
            EmbeddingClient client = new("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            List<string> inputs = new() { text1, text2 };
            OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

            float[] embedding1 = collection[0].ToFloats().ToArray();
            float[] embedding2 = collection[1].ToFloats().ToArray();

            File.WriteAllLines(fileName1 + "embedding_values.csv", embedding1.Select(v => v.ToString()));
            File.WriteAllLines(fileName2 + "embedding_values1.csv", embedding2.Select(v => v.ToString()));

            Console.WriteLine("Scalar values for text1:");
            PrintScalarValues(embedding1);

            Console.WriteLine("Scalar values for text2:");
            PrintScalarValues(embedding2);

            var similarity = CalculateSimilarity(embedding1, embedding2);
            Console.WriteLine($"Embedding1 length: {embedding1.Length}, Embedding2 length: {embedding2.Length}");
            return similarity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating similarity: {ex.Message}");
            return 0;
        }
    }

    private void PrintScalarValues(float[] embedding)
    {
        for (int i = 0; i < embedding.Length; i++)
        {
            Console.WriteLine($"Word {i + 1}: {embedding[i]}");
        }
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

            Console.WriteLine("Magnitude 1 and 2 is" + magnitude1 + "and" + magnitude2);

            double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);
            return cosineSimilarity;
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

    class PhraseSimilarity
    {
        public string? FileName1 { get; set; }
        public string? FileName2 { get; set; }
        public string? domain { get; set; }
        public double SimilarityScore { get; set; }
    }
}
