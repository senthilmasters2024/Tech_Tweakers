using System;
using MySemanticAnalysisSample.SimilarityCalculation;

namespace MySemanticAnalysisSample.SimilarityCalculation
{
    internal class CosineSimilarityCalculator : ISimilarityCalculator
    {
        public float CalculateSimilarity(float[] embedding1, float[] embedding2)
        {
            if (embedding1 == null || embedding2 == null)
                throw new ArgumentNullException("Embeddings cannot be null.");

            if (embedding1.Length != embedding2.Length)
                throw new ArgumentException("Embeddings must have the same length.");

            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                magnitude1 += embedding1[i] * embedding1[i];
                magnitude2 += embedding2[i] * embedding2[i];
            }

            double magnitudeProduct = Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2);

            if (magnitudeProduct == 0)
                throw new InvalidOperationException("Cannot calculate similarity for zero-length vectors.");

            return (float)(dotProduct / magnitudeProduct);
        }
    }
}
