using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAI_API; // Install OpenAI NuGet package: dotnet add package OpenAI

namespace OpenAIEmbeddingExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize the OpenAI API with your API key
            string apiKey = "YOUR_OPENAI_API_KEY"; // Replace with your actual API key
            var openAI = new OpenAIAPI(apiKey);

            // Input text
            string inputText = "Your text string goes here";

            // Generate embeddings
            var embedding = await GenerateEmbedding(openAI, inputText);

            // Output the embedding
            Console.WriteLine("Embedding:");
            Console.WriteLine(string.Join(", ", embedding));
        }

        // Function to generate embeddings
        private static async Task<List<float>> GenerateEmbedding(OpenAIAPI openAI, string inputText)
        {
            var embeddingRequest = new EmbeddingRequest
            {
                Model = "text-embedding-ada-002", // Replace with your preferred model
                Input = new List<string> { inputText }
            };

            var response = await openAI.Embeddings.CreateEmbeddingAsync(embeddingRequest);

            // Extract the embedding from the response
            var embedding = response.Data.FirstOrDefault()?.Embedding;

            return embedding?.ToList() ?? new List<float>();
        }
    }
}
