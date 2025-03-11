using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenAI.Embeddings;
using Plotly.NET.LayoutObjects;
using ScottPlot;
using ScottPlot.Colormaps;
using SemanticaAnalysisTextualData.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class SemanticSimilarityForDocumentsWithInputDataDynamic: ISimilarityService, IEmbedding
{
    public async Task InvokeDocumentComparsion(string[] args)
    {
        // Initialize the service collection and add the SemanticAnalysisTextualDataService
        var services = new ServiceCollection();
        services.AddSingleton<SemanticSimilarityForDocumentsWithInputDataDynamic>(provider => new SemanticSimilarityForDocumentsWithInputDataDynamic());
        var serviceProvider = services.BuildServiceProvider();
        var textAnalysisService = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();

        // Navigate up to the project's root directory from the bin folder
        string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

        // Define the data folder within the project root
        string baseDataFolder = Path.Combine(projectRoot, "data");

        // Define source and target folders dynamically
        string sourceFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnDomains");
        string targetFolder = Path.Combine(baseDataFolder, "PreprocessedSourceBasedOnNeededRelevance");

        // Get all files from the source and target folders
        var sourceFiles = Directory.GetFiles(sourceFolder, "*.txt");
        var targetFiles = Directory.GetFiles(targetFolder, "*.txt");

        var results = new List<PhraseSimilarity>();

        if (textAnalysisService != null)
        {
            try
            {
                var similarityService = new SemanticSimilarityForDocumentsWithInputDataDynamic();
                foreach (var sourceFile in sourceFiles)
                {

                    string fileName1 = Path.GetFileName(sourceFile);
                    Console.WriteLine($"File Name: {fileName1}");

                    string sentence1 = await File.ReadAllTextAsync(sourceFile);

                    foreach (var targetFile in targetFiles)
                    {
                        string fileName2 = Path.GetFileName(targetFile);
                        Console.WriteLine($"File Name: {fileName2}");
                        string sentence2 = await File.ReadAllTextAsync(targetFile);

                        // Calculate the similarity between the two sentences
                        var similarity = await similarityService.CalculateSimilarityAsync(sentence1, sentence2,fileName1,fileName2);
                        Console.WriteLine($"Similarity between {Path.GetFileName(sourceFile)} and {Path.GetFileName(targetFile)}: {similarity:F4}");

                        // Create a PhraseSimilarity object to store the results
                        var phraseSimilarity = new PhraseSimilarity
                        {
                            //Phrase1 = sentence1,
                            //Phrase2 = sentence2,
                            FileName1 = fileName1,
                            FileName2 = fileName2,
                            SimilarityScore = similarity
                        };
                        if (fileName1.StartsWith("preprocessed_JobProfile", StringComparison.OrdinalIgnoreCase))
                        {

                            phraseSimilarity.FileName2 = fileName2;
                            phraseSimilarity.domain = "jobvacancy";
                        }
                        else if (fileName1.StartsWith("preprocessed_MedicalHistory", StringComparison.OrdinalIgnoreCase))
                        {
                            phraseSimilarity.domain = "Medical-MedicationSuggestion";
                        }
                        else
                        {
                            phraseSimilarity.domain = "Unknown"; // Default or fallback domain
                        }
                        Console.WriteLine($"File: {sourceFile}, Domain: {phraseSimilarity.domain}");
                        results.Add(phraseSimilarity);
                    }
                }

                // Save the results to a JSON file
                string currentDir = Directory.GetCurrentDirectory();
                string outputPath = Path.Combine(currentDir, "data", "output_dataset.csv");
                using (var writer = new StreamWriter(outputPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(results);
                }
                Console.WriteLine($"Results saved to {outputPath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // Model for PhraseSimilarity
    class PhraseSimilarity
    {
        public string? FileName1 { get; set; }
        public string? FileName2 { get; set; }

        public string? domain { get; set; }

        public double SimilarityScore { get; set; }
    }

    public async Task<double> CalculateSimilarityAsync(string text1, string text2,String fileName1, String fileName2)
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

        // Fix: Convert ReadOnlyMemory<float> to array before using LINQ methods
        File.WriteAllLines(fileName1+"embedding_values.csv", collection[0].ToFloats().ToArray().Select(v => v.ToString()));
        File.WriteAllLines(fileName2+"embedding_values1.csv", collection[1].ToFloats().ToArray().Select(v => v.ToString()));

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

    public Task<double> CalculateSimilarityAsync(string text1, string text2)
    {
        throw new NotImplementedException();
    }
}
