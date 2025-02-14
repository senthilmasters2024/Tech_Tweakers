﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Services;

namespace SemanticAnalysisTextualData.Source
{
    internal class Program
    {
        /*static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Semantic Text Analysis!");

            // Validate API key
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("The API key environment variable is not set. Please configure it.");

            // Set up dependency injection
            var services = new ServiceCollection();
            services.AddSingleton<SemanticAnalysisTextualDataService>(provider => new SemanticAnalysisTextualDataService());
            var serviceProvider = services.BuildServiceProvider();

            var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();

            *//*while (true)
            {
                Console.WriteLine("Enter text 1: ");
                var text1 = Console.ReadLine();

                Console.WriteLine("Enter text 2: ");
                var text2 = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
                {
                    Console.WriteLine("Both inputs must be non-empty. Please try again.");
                    continue;
                }
*//*
                try
                {
                    if (null != textAnalysisService)
                    {
                    var text1 = "fun";
                    var text2 = "joy";
                    var similarity = await textAnalysisService.CalculateSimilarityAsync(text1, text2);
                        Console.WriteLine($"Similarity: {similarity:F4}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }*/
   }
}
