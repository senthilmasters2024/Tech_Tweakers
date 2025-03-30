using Microsoft.Extensions.DependencyInjection;
using SemanticAnalysisTextualData.Source.Interfaces;
using SemanticAnalysisTextualData.Source.Services;

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
            // Configure services
            var serviceProvider = ConfigureServices();

            // Get required services
            var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();
            var invokeDoc = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            var invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet();

            // Invoking HandlePreProcessing Method for the purpose of getting the user choice whether
            // to decide analysis should run for phrase or documents
            bool isPreProcessRequiredFlag = await PreprocessingHandler.HandlePreprocessing(textPreprocessor);

            // Invoking HandleProcessingChoice Method for the purpose of getting the user choice whether
            // to decide similarity score should generate for phrases or documents
            await ProcessingHandler.HandleProcessingChoice(invokeDoc, invokePhrases, isPreProcessRequiredFlag);

            Console.WriteLine("Process completed.");
        }

        /// <summary>
        /// Configures the services required for the application.
        /// </summary>
        /// <returns>A ServiceProvider instance with the configured services.</returns>
        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<IPreprocessor, TextPreprocessor>()
                .AddSingleton<ISimilarityService, SemanticSimilarityForDocumentsWithInputDataDynamic>()
                .AddSingleton<IEmbedding, SemanticSimilarityForDocumentsWithInputDataDynamic>()
                .BuildServiceProvider();
        }
    }
}
