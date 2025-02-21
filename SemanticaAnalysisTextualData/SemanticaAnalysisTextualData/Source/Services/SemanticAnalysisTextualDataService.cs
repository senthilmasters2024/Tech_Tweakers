using OpenAI.Embeddings;
using ScottPlot.Palettes;
using SemanticaAnalysisTextualData.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.Services
{
    /// <summary>
    /// Service to Implement all our TextualDataSemantic Analsysi Implementations
    /// </summary>
    public class SemanticAnalysisTextualDataService : ISemanticAnalysisTextualDataInterface
    {
        // Create a constructor if needed to initialize anything

        /// <summary>
        /// Asynchronous method to generate embeddings for two text inputs and calculate their similarity.
        /// </summary>
        public async Task<double> CalculateSimilarityAsync(string text1, string text2)
        {
            // Create an EmbeddingClient instance using the OpenAI API key
            EmbeddingClient client = new("text-embedding-3-large" /* Optional: Replace with "text-embedding-3-small" */,
                Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            // Prepare the inputs for embedding generation
            List<string> inputs = new() { text1, text2 };

            // Generate embeddings for the input texts
            OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

            // Extract embeddings for each text
            float[] embedding1 = collection[0].ToFloats().ToArray();
            float[] embedding2 = collection[1].ToFloats().ToArray();

            // Print scalar values of the embeddings to the console
            Console.WriteLine("Scalar values for text1:");
            PrintScalarValues(embedding1);

            Console.WriteLine("Scalar values for text2:");
            PrintScalarValues(embedding2);

            // Calculate similarity between the two embeddings
            var similarity = CalculateSimilarity(embedding1, embedding2);
            Console.WriteLine($"Embedding1 length: {embedding1.Length}, Embedding2 length: {embedding2.Length}");
            return similarity;
        }

        /// <summary>
        /// Prints the scalar values of the embedding to the console.
        /// </summary>
        /// <param name="embedding">The embedding vector as an array of floats.</param>
        private void PrintScalarValues(float[] embedding)
        {
            for (int i = 0; i < embedding.Length; i++)
            {
                Console.WriteLine($"Word {i + 1}: {embedding[i]}");
            }
        }

        /// <summary>
        /// Calculates the cosine similarity between two embeddings.
        /// </summary>
        /// <param name="embedding1">The first embedding vector as an array of floats.</param>
        /// <param name="embedding2">The second embedding vector as an array of floats.</param>
        /// <returns>The cosine similarity score between the two embeddings.</returns>
        /// <exception cref="ArgumentException">Thrown when the embeddings have different lengths or zero magnitude.</exception>
        public double CalculateSimilarity(float[] embedding1, float[] embedding2)
        {
            // Ensure the embeddings have the same length
            if (embedding1.Length != embedding2.Length)
            {
                return 0; // Return 0 if lengths don't match
            }

            double dotProduct = 0.0;  // The dot product of the two vectors
            double magnitude1 = 0.0; // The magnitude of the first vector
            double magnitude2 = 0.0; // The magnitude of the second vector

            // Compute dot product and magnitudes
            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                magnitude1 += Math.Pow(embedding1[i], 2);
                magnitude2 += Math.Pow(embedding2[i], 2);
            }

            // Calculate magnitudes
            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            // Check for zero magnitude
            if (magnitude1 == 0.0 || magnitude2 == 0.0)
            {
                throw new ArgumentException("Embedding vectors must not have zero magnitude.");
            }

            Console.WriteLine("Magnitude 1 and 2 is" + magnitude1 + "and" + magnitude2);
            // Compute cosine similarity
            double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);

            return cosineSimilarity;
        }

        /// <summary>
        /// This is another method to explore how cosine similarity works with vectors
        /// </summary>
        /// <param name="vectorA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double computeCosineSimilarity(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same dimension.");

            double dotProduct = 0.0;
            double magnitudeA = 0.0;
            double magnitudeB = 0.0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);

            return dotProduct / (magnitudeA * magnitudeB);
        }
    }
}
