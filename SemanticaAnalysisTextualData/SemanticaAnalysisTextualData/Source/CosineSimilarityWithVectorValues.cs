using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Interfaces;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source
{
    //internal class CosineSimilarityWithVectorValues : ISemanticAnalysisTextualDataInterface
    //{

    //    static Task CosineSimilarityWithoutEmbedding()
    //    {
    //        var vectorA = new double[] { 1, 2, 3 };
    //        var vectorB = new double[] { 100, 20, 30 };
    //        // Set up dependency injection
    //        var services = new ServiceCollection();
    //        services.AddSingleton(provider => new SemanticAnalysisTextualDataService());
    //        var serviceProvider = services.BuildServiceProvider();
    //        var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();
    //        if (null != textAnalysisService)
    //        {
    //            double cosineSimilarity = textAnalysisService.computeCosineSimilarity(vectorA, vectorB);
    //            Console.WriteLine($"Cosine Similarity: {cosineSimilarity}");
    //        }

    //        return Task.CompletedTask;
    //    }

    //    public double CalculateSimilarity(float[] embedding1, float[] embedding2)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
