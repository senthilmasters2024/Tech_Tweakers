using SemanticAnalysisTextualData.Source.Interfaces;

namespace SemanticAnalysisTextualData.Source.Services
{
   
    /// <summary>
    /// Supports HandleProcessingChoice and PerformPreprocessing for the purpose of getting the user choice whether to proceed analysis with phrases or documents
    /// </summary>
    public static class PreprocessingHandler
    {
        /// <summary>
        /// Handles the preprocessing choice made by the user inorder to provide the user with choice to make decision for proceeding the analysis
        /// with or without preprocessing.
        /// </summary>
        /// <param name="textPreprocessor">The text preprocessor to use.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a flag indicating if preprocessing is required.</returns>
        public static async Task<bool> HandlePreprocessing(IPreprocessor textPreprocessor)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Semantic Analysis without PreProcessing");
            Console.WriteLine("2. Semantic Analysis with PreProcessing");
            var choice = Console.ReadLine();

            if (choice == "2")
            {
                // Perform preprocessing if the user chooses option 2
                return await PerformPreprocessing(textPreprocessor);
            }
            else
            {
                Console.WriteLine("Skipping preprocessing.");
                return false;
            }
        }

        /// <summary>
        /// Performs the preprocessing based on the user's choice to make a decision wheather analysis should run for phrase or documents.
        /// </summary>
        /// <param name="textPreprocessor">The text preprocessor to use.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a flag indicating if preprocessing was performed.</returns>
        private static async Task<bool> PerformPreprocessing(IPreprocessor textPreprocessor)
        {
            Console.WriteLine("Choose what to preprocess:");
            Console.WriteLine("1. Phrases");
            Console.WriteLine("2. Documents");
            var preprocessChoice = Console.ReadLine();

            if (preprocessChoice == "1")
            {
                Console.WriteLine("Preprocessing phrases...");
                // Preprocess phrases (method call commented out)
                // await textPreprocessor.ProcessAndSavePhrasesAsync(phrasesFolder, outputFolder);
                Console.WriteLine("Preprocessing phrases completed.");
                return true;
            }
            else if (preprocessChoice == "2")
            {
                // Preprocess documents
                await DocumentPreprocessor.PreprocessDocuments(textPreprocessor);
                return true;
            }
            else
            {
                Console.WriteLine("Invalid choice. Skipping preprocessing.");
                return false;
            }
        }
    }
}
