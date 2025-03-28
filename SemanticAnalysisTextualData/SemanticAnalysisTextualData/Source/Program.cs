using Microsoft.Extensions.DependencyInjection;
using SemanticAnalysisTextualData.Source.Interfaces;


namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Main program class.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Initialize instances for document and phrase processing
            SemanticSimilarityForDocumentsWithInputDataDynamic invokeDoc = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            SemanticSimilarityPhrasesWithInputDataSet invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet();

            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPreprocessor, TextPreprocessor>() // Use IPreprocessor and TextPreprocessor
                .AddSingleton<ISimilarityService, SemanticSimilarityForDocumentsWithInputDataDynamic>() // Use ISimilarityService and SemanticSimilarityForDocumentsWithInputDataDynamic
                .AddSingleton<IEmbedding, SemanticSimilarityForDocumentsWithInputDataDynamic>() // Use IEmbedding and SemanticSimilarityForDocumentsWithInputDataDynamic
                .BuildServiceProvider();

            // Retrieve services from the service provider
            var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();
            var similarityService = serviceProvider.GetRequiredService<ISimilarityService>();
            var embeddingService = serviceProvider.GetRequiredService<IEmbedding>();
            bool isPreProcessRequiredFlag = false;

            // User options for preprocessing
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

                    // Define source and target folders dynamically
                    string sourceDomainsFolder = Path.Combine(baseDataFolder, "SourceBasedOnDomains");
                    string sourceRelevanceFolder = Path.Combine(baseDataFolder, "SourceBasedOnNeededRelevance");
                    string outputDomainsFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnDomains");
                    string outputRelevanceFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnNeededRelevance");

                    // Ensure output folders exist
                    EnsureDirectoryExists(outputDomainsFolder);
                    EnsureDirectoryExists(outputRelevanceFolder);

                    // Process both source folders
                    await ProcessTextFilesInFolderAsync(textPreprocessor, sourceDomainsFolder, outputDomainsFolder);
                    await ProcessTextFilesInFolderAsync(textPreprocessor, sourceRelevanceFolder, outputRelevanceFolder);

                    Console.WriteLine("Preprocessing for both source folders completed.");
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
                // Invoke phrases comparison
                await invokePhrases.InvokeProcessPhrases();
                Console.WriteLine("Phrases comparison completed.");
            }
            else
            {
                Console.WriteLine("Invalid choice. No further processing.");
            }

            Console.WriteLine("Process completed.");
        }

        /// <summary>
        /// Helper method to ensure a directory exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Processes text files in a folder asynchronously.
        /// </summary>
        /// <param name="textPreprocessor">The text preprocessor to use.</param>
        /// <param name="sourceFolder">The folder containing the source text files.</param>
        /// <param name="outputFolder">The folder to save the processed text files.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async System.Threading.Tasks.Task ProcessTextFilesInFolderAsync(IPreprocessor textPreprocessor, string sourceFolder, string outputFolder)
        {
            Console.WriteLine($"Checking folder: {sourceFolder}");

            // Check if the source folder exists
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Source folder '{sourceFolder}' not found. Skipping.");
                return;
            }

            Console.WriteLine($"Source folder exists: {sourceFolder}");

            // Ensure the output folder exists
            EnsureDirectoryExists(outputFolder);

            // Get all text files in the source folder
            var textFiles = Directory.GetFiles(sourceFolder, "*.txt");

            // Check if there are any text files in the source folder
            if (textFiles.Length == 0)
            {
                Console.WriteLine($"No text files found in '{sourceFolder}'. Skipping.");
                return;
            }

            Console.WriteLine($"Found {textFiles.Length} text files in '{sourceFolder}'. Processing...");

            // Process each text file
            foreach (var file in textFiles)
            {
                string originalFileName = Path.GetFileName(file);
                string newFileName = $"preprocessed_{originalFileName}";  // Add "preprocessed_" prefix
                Console.WriteLine($"Processing file: {originalFileName} → {newFileName}");
                string fileContent = await File.ReadAllTextAsync(file);

                // Check if the file content is empty
                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    Console.WriteLine($"Skipping empty file: {originalFileName}");
                    continue;
                }

                // Preprocess the content
                string preprocessedContent = textPreprocessor.PreprocessText(fileContent, TextDataType.Document);
                string outputFilePath = Path.Combine(outputFolder, newFileName); // Keep the same filename

                // Save the preprocessed content to the output folder
                await File.WriteAllTextAsync(outputFilePath, preprocessedContent);
                Console.WriteLine($"Preprocessed and saved: {outputFilePath}");
            }
        }
    }

}


