using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Services;
using SemanticaAnalysisTextualData.Source.Interfaces;
using Newtonsoft.Json;

namespace SemanticAnalysisTextualData.Source
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Ensure output JSON exists
            string outputJsonPath = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\bin\\Debug\\net8.0\\output_dataset.json";
            EnsureOutputJsonExists(outputJsonPath);

            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITextPreprocessor, TextPreprocessor>() // Register text preprocessor
                .AddSingleton<ISemanticAnalysisTextualDataInterface, SemanticAnalysisTextualDataService>()
                .BuildServiceProvider();

            var semanticService = serviceProvider.GetRequiredService<ISemanticAnalysisTextualDataInterface>();

            // **Step 1: Fetch Inputs**
            Console.WriteLine("Fetching input data...");
            string wordsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Words";
            string phrasesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Phrases";

            CheckAndReadFiles(wordsFolder);
            CheckAndReadFiles(phrasesFolder);

            // **Step 2: Generate Embeddings**
            Console.WriteLine("Generating embeddings for fetched data...");
            await semanticService.GenerateEmbeddingsForWordsAndPhrases(wordsFolder, phrasesFolder);

            Console.WriteLine("Embedding analysis completed.");
        }

        // **Ensure `output_dataset.json` Exists**
        static void EnsureOutputJsonExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"⚠️ `output_dataset.json` not found. Creating an empty JSON file...");
                File.WriteAllText(filePath, "{}"); // Creates an empty JSON file
            }
            else
            {
                Console.WriteLine($"✅ `output_dataset.json` found. Proceeding...");
            }
        }

        // **Helper method to check and read files**
        static void CheckAndReadFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"❌ Directory not found: {folderPath}");
                return;
            }

            var files = Directory.GetFiles(folderPath, "*.txt");
            if (!files.Any())
            {
                Console.WriteLine($"No text files found in {folderPath}");
                return;
            }

            Console.WriteLine($" Found {files.Length} text files in {folderPath}");

            foreach (var file in files)
            {
                Console.WriteLine($" Reading file: {file}");
                string content = File.ReadAllText(file);
                Console.WriteLine($" File Content (first 200 chars): {content.Substring(0, Math.Min(content.Length, 200))}...\n");
            }
        }
    }
}

/*using System;
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
                .AddSingleton<ITextPreprocessor, TextPreprocessor>() // Registering the unified text preprocessor
                .AddSingleton<ISemanticAnalysisTextualDataInterface, SemanticAnalysisTextualDataService>()
                .BuildServiceProvider();

            var semanticService = serviceProvider.GetRequiredService<ISemanticAnalysisTextualDataInterface>();


            // Define paths for document processing
            //string wordsFolder = @"D:\path\to\wordsFolder";
            //string phrasesFolder = @"D:\path\to\phrasesFolder";

            //string outputWords = @"D:\path\to\words-output";
            //string outputPhrases = @"D:\path\to\phrases-output";
            // Check if documents exist before processing
            Console.WriteLine("Checking document files...");
            string requirementsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\docstobecompared\\Requirements Folder";
            string resumesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\docstobecompared\\Resume Folder";

            // Check if files exist in the given folders
            CheckAndReadFiles(requirementsFolder);
            CheckAndReadFiles(resumesFolder);
            static void CheckAndReadFiles(string folderPath)
            {
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine($"❌ Directory not found: {folderPath}");
                    return;
                }

                var files = Directory.GetFiles(folderPath, "*.txt");
                if (!files.Any())
                {
                    Console.WriteLine($"No text files found in {folderPath}");
                    return;
                }

                Console.WriteLine($" Found {files.Length} text files in {folderPath}");

                foreach (var file in files)
                {
                    Console.WriteLine($" Reading file: {file}");
                    string content = File.ReadAllText(file);
                    Console.WriteLine($" File Content (first 200 chars): {content.Substring(0, Math.Min(content.Length, 200))}...\n");
                }
            }


            string outputRequirements = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\docstobecompared\\Requirements Folder";
            string outputResumes = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\docstobecompared\\Resume Folder";
            // Preprocess words and phrases
           // Console.WriteLine("Starting word and phrase preprocessing...");
            //await semanticService.PreprocessWordsAndPhrases(wordsFolder, phrasesFolder, outputWords, outputPhrases);

            // Preprocess documents (if needed)
            Console.WriteLine("Starting document preprocessing...");
            await semanticService.PreprocessAllDocuments(requirementsFolder, resumesFolder, outputRequirements, outputResumes);

            // Perform similarity calculations for words and phrases
            //Console.WriteLine("Starting similarity calculations for words and phrases...");
           // await semanticService.CalculateSimilarityForWordsAndPhrasesAsync(outputWords, outputPhrases);

            // Perform similarity calculations for documents (job descriptions vs resumes)
            Console.WriteLine("Starting similarity calculations for documents...");
            await semanticService.CalculateSimilarityForDocumentsAsync(outputRequirements, outputResumes);

            Console.WriteLine("Process completed.");
        }
    }
}
/*


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

