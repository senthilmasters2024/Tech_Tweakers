using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemanticAnalysisTextualData.Source.Interfaces;
using SemanticAnalysisTextualData.Source.Services;

namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Main program class.
    /// </summary>
    class Program
    {

        // Configure services
        public static async System.Threading.Tasks.Task Main(string[] args)
        {

             
        var serviceProvider = ConfigureServices();

            // Get required services
            var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();
            var invokeDoc = serviceProvider.GetRequiredService<ISimilarityService>()
     as SemanticSimilarityForDocumentsWithInputDataDynamic;
            // Pass logger to constructor
            var invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet();

            bool isPreProcessRequiredFlag = await PreprocessingHandler.HandlePreprocessing(textPreprocessor);
            await ProcessingHandler.HandleProcessingChoice(invokeDoc, invokePhrases, isPreProcessRequiredFlag);

            Console.WriteLine("Process completed.");
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(config =>
                {
                    config.AddConsole(); // ✅ Console logger
                    config.SetMinimumLevel(LogLevel.Information);
                })
                .AddSingleton<IPreprocessor, TextPreprocessor>()
                .AddSingleton<ISimilarityService, SemanticSimilarityForDocumentsWithInputDataDynamic>()
                .AddSingleton<IEmbedding, SemanticSimilarityForDocumentsWithInputDataDynamic>()
                .BuildServiceProvider();
        }

    }
}
