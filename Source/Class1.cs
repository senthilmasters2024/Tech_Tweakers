using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Services;

namespace SemanticAnalysisTextualData.Source
{
    //internal class Programa
    //{
         /*static async Task Main(string[] args)
         {
             Console.WriteLine("Welcome to Semantic Text Analysis!");

             // Validate API key
             var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
             if (string.IsNullOrEmpty(apiKey))
                 throw new InvalidOperationException("The API key environment variable is not set. Please configure it.");

             // Set up dependency injection
             var services = new ServiceCollection();
             services.AddSingleton<SemanticAnalysisTextualDataDocumentService>(provider => new SemanticAnalysisTextualDataDocumentService());
             var serviceProvider = services.BuildServiceProvider();

             var docAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataDocumentService>();



                 try
                 {
                     if (null != docAnalysisService)
                     {
                     Task<double>  abc=docAnalysisService.CalculateSimilarityAsync();
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error: {ex.Message}");
                 }
             }
         }*/
    //}
}
