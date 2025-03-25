using Microsoft.Extensions.DependencyInjection;
using SemanticAnalysisTextualData.Source.Interfaces;


namespace SemanticAnalysisTextualData.Source
{
    class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
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
            bool isPreProcessRequiredFlag = false;

            // User options
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Semantic Analysis without PreProcessing");
            Console.WriteLine("2. Semantic Analysis with PreProcessing");
            var choice = Console.ReadLine();

            if (choice == "2")
            {
                Console.WriteLine("Choose what to preprocess:");
                Console.WriteLine("1. Phrases");
                Console.WriteLine("2. Documents");
                var preprocessChoice = Console.ReadLine();

                if (preprocessChoice == "1")
                {
                    isPreProcessRequiredFlag = true;
                    // Preprocess Phrases
                    Console.WriteLine("Preprocessing phrases...");
                    // await textPreprocessor.ProcessAndSavePhrasesAsync(phrasesFolder, outputFolder);
                    Console.WriteLine("Preprocessing phrases completed.");
                }
                else if (preprocessChoice == "2")
                {
                    isPreProcessRequiredFlag = true;
                    // Navigate up to the project's root directory from the bin folder
                    string? projectRoot = AppContext.BaseDirectory ?? throw new InvalidOperationException("Unable to determine project root directory.");

                    // Define the data folder within the project root
                    string baseDataFolder = Path.Combine(projectRoot, "data");
                    // var config = new ConfigurationBuilder()
                    // .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the project directory
                    // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load the JSON file
                    // .Build();
                    // Define source and target folders dynamically
                    string sourceDomainsFolder = Path.Combine(baseDataFolder, "SourceBasedOnDomains");
                    string sourceRelevanceFolder = Path.Combine(baseDataFolder, "SourceBasedOnNeededRelevance");
                    // Get folder paths from configuration
                    string outputDomainsFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnDomains");
                    string outputRelevanceFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnNeededRelevance");

                    // Ensure output folders exist
                    EnsureDirectoryExists(outputDomainsFolder);
                    EnsureDirectoryExists(outputRelevanceFolder);

                    // Process both source folders
                    await ProcessTextFilesInFolderAsync(textPreprocessor, sourceDomainsFolder, outputDomainsFolder);
                    await ProcessTextFilesInFolderAsync(textPreprocessor, sourceRelevanceFolder, outputRelevanceFolder);

                    Console.WriteLine(" Preprocessing for both source folders completed.");
                    // Preprocess Documents
                    Console.WriteLine("Preprocessing documents...");
                    //await textPreprocessor.ProcessAndSaveDocumentsAsync(documentsFolder, outputFolder);
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
                await invokeDoc.InvokeDocumentComparsion(isPreProcessRequiredFlag);
                Console.WriteLine("Document comparison completed.");
            }
            else if (processChoice == "2")
            {
                // Example: Calculate similarity between two sample phrases
                await invokePhrases.InvokeProcessPhrases();
                Console.WriteLine("Phrases comparison completed.");
            }
            else
            {
                Console.WriteLine("Invalid choice. No further processing.");
            }

            Console.WriteLine("Process completed.");
        }

        // Helper method to ensure a directory exists
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static async System.Threading.Tasks.Task ProcessTextFilesInFolderAsync(IPreprocessor textPreprocessor, string sourceFolder, string outputFolder)
        {
            Console.WriteLine($" Checking folder: {sourceFolder}");

            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($" Source folder '{sourceFolder}' not found. Skipping.");
                return;
            }

            Console.WriteLine($" Source folder exists: {sourceFolder}");

            // Ensure output folder exists
            EnsureDirectoryExists(outputFolder);

            var textFiles = Directory.GetFiles(sourceFolder, "*.txt");

            if (textFiles.Length == 0)
            {
                Console.WriteLine($" No text files found in '{sourceFolder}'. Skipping.");
                return;
            }

            Console.WriteLine($" Found {textFiles.Length} text files in '{sourceFolder}'. Processing...");

            foreach (var file in textFiles)
            {
                string originalFileName = Path.GetFileName(file);
                string newFileName = $"preprocessed_{originalFileName}";  //  Add "preprocessed_" prefix
                Console.WriteLine($" Processing file: {originalFileName} → {newFileName}");
                string fileContent = await File.ReadAllTextAsync(file);

                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    Console.WriteLine($" Skipping empty file: {originalFileName}");
                    continue;
                }

                //  Preprocess the content
                string preprocessedContent = textPreprocessor.PreprocessText(fileContent, TextDataType.Document);
                string outputFilePath = Path.Combine(outputFolder, newFileName); //  Keep the same filename

                await File.WriteAllTextAsync(outputFilePath, preprocessedContent);
                Console.WriteLine($" Preprocessed and saved: {outputFilePath}");
            }
        }
    }

}


