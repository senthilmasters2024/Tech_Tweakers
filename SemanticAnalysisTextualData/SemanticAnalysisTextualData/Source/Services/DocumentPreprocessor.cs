using SemanticAnalysisTextualData.Source.Interfaces;

namespace SemanticAnalysisTextualData.Source
{
   
  /// <summary>
  /// Provides methods for preprocessing documents by reading from source folders and saving to output folders.
  /// </summary>
  public static class DocumentPreprocessor
    {
        /// <summary>
        /// Preprocesses the documents by reading from source folders and saving to output folders.
        /// </summary>
        /// <param name="textPreprocessor">The text preprocessor to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task PreprocessDocuments(IPreprocessor textPreprocessor)
        {
            // Determine project root directory
            string? projectRoot = AppContext.BaseDirectory ?? throw new InvalidOperationException("Unable to determine project root directory.");
            string baseDataFolder = Path.Combine(projectRoot, "data");

            // Define source and output folders
            string sourceDomainsFolder = Path.Combine(baseDataFolder, "SourceBasedOnDomains");
            string sourceRelevanceFolder = Path.Combine(baseDataFolder, "SourceBasedOnNeededRelevance");
            string outputDomainsFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnDomains");
            string outputRelevanceFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnNeededRelevance");

            // Ensure output directories exist
            DirectoryHelper.EnsureDirectoryExists(outputDomainsFolder);
            DirectoryHelper.EnsureDirectoryExists(outputRelevanceFolder);

            // Preprocess text files in source folders
            await PreProcessTextFilesInFolderAsync(textPreprocessor, sourceDomainsFolder, outputDomainsFolder);
            await PreProcessTextFilesInFolderAsync(textPreprocessor, sourceRelevanceFolder, outputRelevanceFolder);

            Console.WriteLine("Preprocessing for both source folders completed.");
            Console.WriteLine("Preprocessing documents...");
            // Preprocess documents (method call commented out)
            // await textPreprocessor.ProcessAndSaveDocumentsAsync(documentsFolder, outputFolder);
            Console.WriteLine("Preprocessing documents completed.");
        }

        /// <summary>
        /// Preprocesses text files into an output folder asynchronously.
        /// PreProcess means in order to remove stop word removal, pre-defined articles removals and to do process of lemmatization
        /// </summary>
        /// <param name="textPreprocessor">The text preprocessor to use.</param>
        /// <param name="sourceFolder">The folder containing the source raw text files.</param>
        /// <param name="outputFolder">The folder to save the processed raw text files.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task PreProcessTextFilesInFolderAsync(IPreprocessor textPreprocessor, string sourceFolder, string outputFolder)
        {
            Console.WriteLine($"Checking folder: {sourceFolder}");

            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Source folder '{sourceFolder}' not found. Skipping.");
                return;
            }

            Console.WriteLine($"Source folder exists: {sourceFolder}");

            //Invoke EnsureDirectoryExists to Ensure if directory exists and create if its not present
            DirectoryHelper.EnsureDirectoryExists(outputFolder);

            var textFiles = Directory.GetFiles(sourceFolder, "*.txt");

            if (textFiles.Length == 0)
            {
                Console.WriteLine($"No text files found in '{sourceFolder}'. Skipping.");
                return;
            }

            Console.WriteLine($"Found {textFiles.Length} text files in '{sourceFolder}'. Processing...");

            foreach (var file in textFiles)
            {
                string originalFileName = Path.GetFileName(file);
                string newFileName = $"preprocessed_{originalFileName}";
                Console.WriteLine($"Processing file: {originalFileName} → {newFileName}");
                string fileContent = await File.ReadAllTextAsync(file);

                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    Console.WriteLine($"Skipping empty file: {originalFileName}");
                    continue;
                }

                // Preprocess text content by invoking PreprocessText method for documents
                string preprocessedContent = textPreprocessor.PreprocessText(fileContent, TextDataType.Document);
                string outputFilePath = Path.Combine(outputFolder, newFileName);

                // Save preprocessed content in a predefined output folder
                await File.WriteAllTextAsync(outputFilePath, preprocessedContent);
                Console.WriteLine($"Preprocessed and saved: {outputFilePath}");
            }
        }
    }
}
