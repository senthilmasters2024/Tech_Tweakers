
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Services;
using SemanticaAnalysisTextualData.Source.Interfaces;


namespace SemanticAnalysisTextualData.Source
{


    class Program
    {

        static async Task Main(string[] args)
        {

            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IWordPreprocessor, WordPreprocessor>()  // Replace with actual implementations
                .AddSingleton<ISentencePreprocessor, SentencePreprocessor>()
                .AddSingleton<IDocumentPreprocessor, DocumentPreprocessor>()
                .AddSingleton<ISemanticAnalysisTextualDataInterface, SemanticAnalysisTextualDataService>()
                .BuildServiceProvider();

            var semanticService = serviceProvider.GetRequiredService<ISemanticAnalysisTextualDataInterface>();

            // Define paths for document processing
            string requirementsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Requirements Folder";
            string resumesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Resume Folder";
            string outputRequirements = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Requirements Folder-output";
            string outputResumes = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Resume Folder-output";

            // Preprocess documents
            Console.WriteLine("Starting document preprocessing...");
            await Task.Run(() => semanticService.PreprocessAllDocuments(requirementsFolder, resumesFolder, outputRequirements, outputResumes));

            // Define processed document paths
            //string processedRequirementsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Requirements Folder-preprocessed";
            //string processedResumesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Resume Folder-output-preprocessed";

            // Perform similarity calculations
            Console.WriteLine("Starting similarity calculations...");
            await semanticService.CalculateSimilarityForDocumentsAsync(outputRequirements, outputResumes);

            Console.WriteLine("Process completed.");
        }
    }
}


        /*static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Semantic Text Analysis!");

            // Validate API key
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("The API key environment variable is not set. Please configure it.");

            // Set up dependency injection
            var services = new ServiceCollection();
            services.AddSingleton<SemanticAnalysisTextualDataService>(provider => new SemanticAnalysisTextualDataService());
            var serviceProvider = services.BuildServiceProvider();

            var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();

            *//*while (true)
            {
                Console.WriteLine("Enter text 1: ");
                var text1 = Console.ReadLine();

                Console.WriteLine("Enter text 2: ");
                var text2 = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
                {
                    Console.WriteLine("Both inputs must be non-empty. Please try again.");
                    continue;
                }
*//*
                try
                {
                    if (null != textAnalysisService)
                    {
                    var text1 = "fun";
                    var text2 = "joy";
                    var similarity = await textAnalysisService.CalculateSimilarityAsync(text1, text2);
                        Console.WriteLine($"Similarity: {similarity:F4}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }*/
   
