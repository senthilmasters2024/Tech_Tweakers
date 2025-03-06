using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Services;
using SemanticaAnalysisTextualData.Source.Interfaces;
using Google.Apis.Util.Store;

using System.ComponentModel;
using System.Diagnostics;

namespace SemanticAnalysisTextualData.Source
{
    class Program
    {
       /* static async Task Main(string[] args)
        {
            // Set up dependency injection
         
           
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPreprocessor, TextPreprocessor>() // Use IPreprocessor and TextPreprocessor
                .BuildServiceProvider();


            var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();

            // **Step 1: Define Input and Output Folders**
            string wordsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Input Data\\Words";
            string phrasesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Input Data\\Phrases";
            string documentsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Input Data\\Documents";

            string outputFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
            Console.WriteLine($"Using Output Folder: {outputFolder}"); // Debugging output


            // Ensure output folders exist
            EnsureDirectoryExists(outputFolder);
            EnsureDirectoryExists(Path.Combine(outputFolder, "Words"));
            EnsureDirectoryExists(Path.Combine(outputFolder, "Phrases"));
            EnsureDirectoryExists(Path.Combine(outputFolder, "Documents"));

            //  Preprocess Words
            // Preprocess Words

            // Preprocess Words (Process each domain inside Words folder)
            foreach (var domainFolder in Directory.GetDirectories(wordsFolder))
            {
                string domainName = new DirectoryInfo(domainFolder).Name;
                Console.WriteLine($"Preprocessing words for domain: {domainName}...");
                await textPreprocessor.ProcessAndSaveWordsAsync(domainName, wordsFolder, outputFolder);
            }
            // Preprocess Phrases(Process each domain inside Phrases folder)
              foreach (var domainFolder in Directory.GetDirectories(phrasesFolder))
            {
                string domainName = new DirectoryInfo(domainFolder).Name;
                Console.WriteLine($"Preprocessing phrases for domain: {domainName}...");
                await textPreprocessor.ProcessAndSavePhrasesAsync(domainName, phrasesFolder, outputFolder);
            }
            

            // Preprocess Documents
            Console.WriteLine("Preprocessing documents...");
            await textPreprocessor.ProcessAndSaveDocumentsAsync("Requirement", documentsFolder, outputFolder);
            await textPreprocessor.ProcessAndSaveDocumentsAsync("Resume", documentsFolder, outputFolder);


            //  Display Preprocessed Data
            //Console.WriteLine("\nPreprocessed Words:");
            //DisplayPreprocessedFiles(outputFolder, "Words");

            //Console.WriteLine("\nPreprocessed Phrases:");
            //DisplayPreprocessedFiles(outputFolder, "Phrases");

            //Console.WriteLine("\nPreprocessed Documents:");
            //DisplayPreprocessedFiles(outputFolder, "Documents");

            Console.WriteLine("Preprocessing completed.");
        }
        // **Helper method to ensure a directory exists**
        static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        // **Helper method to display preprocessed files**
        static void DisplayPreprocessedFiles(string folderPath, string category)
        {
            string categoryPath = Path.Combine(folderPath, category);
            if (!Directory.Exists(categoryPath))
            {
                Console.WriteLine($"No preprocessed files found in {categoryPath}");
                return;
            }

            var files = Directory.GetFiles(categoryPath, "*.txt");
            if (!files.Any())
            {
                Console.WriteLine($"No preprocessed files found in {categoryPath}");
                return;
            }

            foreach (var file in files)
            {
                Console.WriteLine($"File: {Path.GetFileName(file)}");
                string content = File.ReadAllText(file);
                Console.WriteLine($"Content: {content}\n");
            }
        }
    }
}

/* 

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
                //..AddSingleton<ISemanticAnalysisTextualDataInterface, SemanticAnalysisTextualDataService>()
                .BuildServiceProvider();

            var semanticService = serviceProvider.GetRequiredService<ISemanticAnalysisTextualDataInterface>();

            // **Step 1: Fetch Inputs**
            Console.WriteLine("Fetching input data...");
            string wordsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Words";
            string phrasesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Data\\Phrases";

            CheckAndReadFiles(wordsFolder);
            CheckAndReadFiles(phrasesFolder);
*/

/*

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


  */

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

