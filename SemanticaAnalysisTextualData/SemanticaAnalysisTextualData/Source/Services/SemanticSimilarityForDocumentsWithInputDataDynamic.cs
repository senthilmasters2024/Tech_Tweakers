using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class SemanticSimilarityForDocumentsWithInputDataDynamic
{
    static async Task Main(string[] args)
    {
        // Initialize the service collection and add the SemanticAnalysisTextualDataService
        var services = new ServiceCollection();
        services.AddSingleton<SemanticAnalysisTextualDataService>(provider => new SemanticAnalysisTextualDataService());
        var serviceProvider = services.BuildServiceProvider();
        var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();

        // Define the source and target folders
        string sourceFolder = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\JobRequirements";
        string targetFolder = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\JobProfiles";

        // Get all files from the source and target folders
        var sourceFiles = Directory.GetFiles(sourceFolder, "*.txt");
        var targetFiles = Directory.GetFiles(targetFolder, "*.txt");

        var results = new List<PhraseSimilarity>();

        if (textAnalysisService != null)
        {
            try
            {
                foreach (var sourceFile in sourceFiles)
                {
                    string sentence1 = await File.ReadAllTextAsync(sourceFile);

                    foreach (var targetFile in targetFiles)
                    {
                        string sentence2 = await File.ReadAllTextAsync(targetFile);

                        // Calculate the similarity between the two sentences
                        var similarity = await textAnalysisService.CalculateSimilarityAsync(sentence1, sentence2);
                        Console.WriteLine($"Similarity between {Path.GetFileName(sourceFile)} and {Path.GetFileName(targetFile)}: {similarity:F4}");

                        // Create a PhraseSimilarity object to store the results
                        var phraseSimilarity = new PhraseSimilarity
                        {
                            Phrase1 = sentence1,
                            Phrase2 = sentence2,
                            SimilarityScore = similarity
                        };

                        results.Add(phraseSimilarity);
                    }
                }

                // Save the results to a JSON file
                string currentDir = Directory.GetCurrentDirectory();
                string outputPath = Path.Combine(currentDir, "data", "output_dataset.json");
                File.WriteAllText(outputPath, JsonConvert.SerializeObject(results, Formatting.Indented));
                Console.WriteLine($"Results saved to {outputPath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // Model for PhraseSimilarity
    class PhraseSimilarity
    {
        public string? Phrase1 { get; set; }
        public string? Phrase2 { get; set; }
        public double SimilarityScore { get; set; }
    }
}
