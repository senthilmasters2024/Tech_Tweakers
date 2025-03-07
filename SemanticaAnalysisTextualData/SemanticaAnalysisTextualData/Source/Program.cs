using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Interfaces;


    namespace SemanticAnalysisTextualData.Source
    {
        class Program
        {
            public static async Task Main(string[] args)
            {
                SemanticSimilarityForDocumentsWithInputDataDynamic invokeDoc = new SemanticSimilarityForDocumentsWithInputDataDynamic();
                SemanticSimilarityPhrasesWithInputDataSet invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet();
                // Set up dependency injection
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IPreprocessor, TextPreprocessor>() // Use IPreprocessor and TextPreprocessor
                    .AddSingleton<ISimilarityService, SemanticSimilarityForDocumentsWithInputDataDynamic>() // Use ISimilarityService and SemanticSimilarityForDocumentsWithInputDataDynamic
                    .AddSingleton<IEmbedding, SemanticSimilarityForDocumentsWithInputDataDynamic>() // Use IEmbedding and SemanticSimilarityForDocumentsWithInputDataDynamic
                    .BuildServiceProvider();

                var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();
                var similarityService = serviceProvider.GetRequiredService<ISimilarityService>();
                var embeddingService = serviceProvider.GetRequiredService<IEmbedding>();

                // Define Input and Output Folders
                string phrasesFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Input Data\\Phrases";
                string documentsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\Input Data\\Documents";
                string outputFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
                Console.WriteLine($"Using Output Folder: {outputFolder}"); // Debugging output

                // Ensure output folders exist
                EnsureDirectoryExists(outputFolder);
                EnsureDirectoryExists(Path.Combine(outputFolder, "Words"));
                EnsureDirectoryExists(Path.Combine(outputFolder, "Phrases"));
                EnsureDirectoryExists(Path.Combine(outputFolder, "Documents"));

                // User options
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Skip Preprocessing");
                Console.WriteLine("2. Apply Preprocessing");
                var choice = Console.ReadLine();

                if (choice == "2")
                {
                    Console.WriteLine("Choose what to preprocess:");
                    Console.WriteLine("1. Phrases");
                    Console.WriteLine("2. Documents");
                    var preprocessChoice = Console.ReadLine();

                    if (preprocessChoice == "1")
                    {
                        // Preprocess Phrases
                        Console.WriteLine("Preprocessing phrases...");
                        await textPreprocessor.ProcessAndSavePhrasesAsync(phrasesFolder, outputFolder);
                        Console.WriteLine("Preprocessing phrases completed.");
                    }
                    else if (preprocessChoice == "2")
                    {
                        // Preprocess Documents
                        Console.WriteLine("Preprocessing documents...");
                        await textPreprocessor.ProcessAndSaveDocumentsAsync(documentsFolder, outputFolder);
                        Console.WriteLine("Preprocessing documents completed.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Skipping preprocessing.");
                    }
                }
                else
                {
                    Console.WriteLine("Skipping preprocessing.");
                }
                // Additional user choice for document processing or phrases comparison
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Process Documents");
                Console.WriteLine("2. Compare Phrases");
                var processChoice = Console.ReadLine();

                if (processChoice == "1")
                {
                    // Invoke document comparison
                    Console.WriteLine("Invoking document comparison...");
                    await invokeDoc.InvokeDocumentComparsion(args);
                    Console.WriteLine("Document comparison completed.");
                }
                else if (processChoice == "2")
                {
                    // Example: Calculate similarity between two sample phrases
                    await invokePhrases.invokeProcessPhrases(args);
                    Console.WriteLine("Phrases comparison completed.");
                }
                else
                {
                    Console.WriteLine("Invalid choice. No further processing.");
                }

                Console.WriteLine("Process completed.");
            }

            // Helper method to ensure a directory exists
            static void EnsureDirectoryExists(string path)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }
    }
     
     
