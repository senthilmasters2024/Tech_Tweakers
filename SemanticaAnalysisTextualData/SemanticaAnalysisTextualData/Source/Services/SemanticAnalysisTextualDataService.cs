﻿using OpenAI.Embeddings;
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
      
        //Create a constructor If needed to Initialise Anything

        /// <summary>
        /// Asynchronous method to generate embeddings for two text inputs and calculate their similarity.
        /// </summary>
        public async Task<double> CalculateSimilarityAsync(string text1, string text2)
        {
            // Create an EmbeddingClient instance using the OpenAI API key
            EmbeddingClient client = new("text-embedding-3-small" /* Optional: Replace with "text-embedding-3-small" */,
                Environment.GetEnvironmentVariable("OPENAI_API_KEY"));


            // Infinite loop to allow repeated similarity calculations
            while (true)
            {
                // Prompt the user to input the first text
                Console.WriteLine("Enter text 1: ");
                var inp1 = Console.ReadLine();

                // Prompt the user to input the second text
                Console.WriteLine("Enter text 2: ");
                var inp2 = Console.ReadLine();

                // Validate user input
                if (string.IsNullOrWhiteSpace(inp1) || string.IsNullOrWhiteSpace(inp2))
                {
                    Console.WriteLine("Both inputs must be non-empty. Please try again.");
                    continue;
                }

                // Prepare the inputs for embedding generation
                List<string> inputs = new() { inp1, inp2 };

                // Generate embeddings for the input texts
                OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

                // Calculate similarity between the two embeddings
                var similarity = CalculateSimilarity(
                    collection[0].ToFloats().ToArray(),
                    collection[1].ToFloats().ToArray()
                );

                // Display the similarity score
                Console.WriteLine($"Similarity: {similarity:F4}");
                Console.WriteLine();
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

            // Compute cosine similarity
            double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);

            return cosineSimilarity;
        }

        
     
    }
}
