/*using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class SemanticSimilarityDocumentsWithInputDataSet
{
   /* static async Task Main(string[] args)
    {
        // Initialize the service collection and add the SemanticAnalysisTextualDataService
        var services = new ServiceCollection();
        services.AddSingleton<SemanticAnalysisTextualDataService>(provider => new SemanticAnalysisTextualDataService());
        var serviceProvider = services.BuildServiceProvider();
        var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();

        // Read the contents of the two text files
        string sentence1Path = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\JobRequirement.txt";
        string sentence2Path = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\JobProfileADevOps.txt";

        string sentence1 = await File.ReadAllTextAsync(sentence1Path);
        string sentence2 = await File.ReadAllTextAsync(sentence2Path);

        // Calculate the similarity between the two sentences
        if (textAnalysisService != null)
        {
            try
            {
                var similarity = await CalculateSimilarityAsync(textAnalysisService, sentence1, sentence2);
                Console.WriteLine($"Similarity: {similarity:F4}");

                // Create a PhraseSimilarity object to store the results
                var phraseSimilarity = new PhraseSimilarity
                {
                    Phrase1 = sentence1,
                    Phrase2 = sentence2,
                    SimilarityScore = similarity
                };

                // Save the results to a JSON file
                string currentDir = Directory.GetCurrentDirectory();
                string outputPath = Path.Combine(currentDir, "data", "output_dataset.json");
                File.WriteAllText(outputPath, JsonConvert.SerializeObject(phraseSimilarity, Formatting.Indented));
                Console.WriteLine($"Results saved to {outputPath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }*/


    // Method to calculate similarity
    private static async Task<double> CalculateSimilarityAsync(SemanticAnalysisTextualDataService service, string sentence1, string sentence2)
    {
        // Assuming the service has a method to preprocess and calculate similarity
        service.PreprocessWordsAndPhrases(sentence1, sentence2, "outputWords", "outputPhrases");
        await service.CalculateSimilarityForDocumentsAsync("outputWords", "outputPhrases");
        // Dummy similarity score for demonstration
        return 0.85;
    }

    // Model for PhraseSimilarity
    class PhraseSimilarity
    {
        public string? Phrase1 { get; set; }
        public string? Phrase2 { get; set; }
        public double SimilarityScore { get; set; }
    }
