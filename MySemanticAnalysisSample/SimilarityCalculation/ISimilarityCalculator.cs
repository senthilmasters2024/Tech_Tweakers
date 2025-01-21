using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySemanticAnalysisSample.SimilarityCalculation
{
    internal interface ISimilarityCalculator
    {
        float CalculateSimilarity(float[] embedding1, float[] embedding2);
       
    }
}
