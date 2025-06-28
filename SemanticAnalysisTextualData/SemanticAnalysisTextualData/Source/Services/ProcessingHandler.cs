using SemanticAnalysisTextualData.Source.Interfaces;

namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Procesing Handler inorder to handle the processing choice made by the user
    /// </summary>
    public static class ProcessingHandler
    {
        /// <summary>
        /// Handles the processing choice made by the user.
        /// </summary>
        /// <param name="invokeDoc">The document similarity service to use.</param>
        /// <param name="invokePhrases">The phrase similarity service to use.</param>
        /// <param name="isPreProcessRequiredFlag">Flag indicating if preprocessing is required.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task HandleProcessingChoice(SemanticSimilarityForDocumentsWithInputDataDynamic invokeDoc,
                                                        SemanticSimilarityPhrasesWithInputDataSet invokePhrases, bool isPreProcessRequiredFlag)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Process Documents");
            Console.WriteLine("2. Compare Phrases");
            var processChoice = Console.ReadLine();

            if (processChoice == "1")
            {
                Console.WriteLine("Invoking document comparison...");
                // Invoke document comparison analysis
                await invokeDoc.InvokeDocumentComparsion(isPreProcessRequiredFlag);
                Console.WriteLine("Document comparison completed.");
            }
            else if (processChoice == "2")
            {
                // Invoke phrase comparison analysis
                await invokePhrases.InvokeProcessPhrases();
                Console.WriteLine("Phrases comparison completed.");
            }
            else
            {
                Console.WriteLine("Invalid choice. No further processing.");
            }
        }
    }
}
