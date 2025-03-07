using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenAI.Embeddings;
using ScottPlot.Colormaps;
using SemanticaAnalysisTextualData.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using static Microsoft.FSharp.Core.ByRefKinds;

class SemanticSimilarityPhrasesWithInputDataSet:ISimilarityService
{
    public async Task invokeProcessPhrases(string[] args)
    {
        // Create an EmbeddingClient instance using the OpenAI API key
        EmbeddingClient client = new("text-embedding-3-large" /* Optional: Replace with "text-embedding-3-small" */,
            Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        // Navigate up to the project's root directory from the bin folder
        string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

        // Define the data folder within the project root
        string baseDataFolder = Path.Combine(projectRoot, "data");

        // Define source and target folders dynamically
        string datasetPath = Path.Combine(baseDataFolder, "InputPhrases50DataSet.json");
        // Load predefined dataset
        //string datasetPath = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\InputPhrases50DataSet.json";
        var dataset = JsonConvert.DeserializeObject<InputDataset>(File.ReadAllText(datasetPath));
        var services = new ServiceCollection();
        services.AddSingleton<SemanticSimilarityPhrasesWithInputDataSet>(provider => new SemanticSimilarityPhrasesWithInputDataSet());
        var serviceProvider = services.BuildServiceProvider();
        var textAnalysisService = serviceProvider.GetService<SemanticSimilarityPhrasesWithInputDataSet>();
        List<PhraseSimilarity> PhrasePairs = new List<PhraseSimilarity>();
        InputDataset obj = new InputDataset();
        var results = new List<PhraseSimilarity>();
        // Process phrase pairs
        if (null != dataset)
        {
            foreach (var pair in dataset.PhrasePairs)
            {
                try
                {
                    
                    if (null != textAnalysisService && null != pair.Phrase1 && null != pair.Phrase2)
                    {

                        // Prepare the inputs for embedding generation
                        List<string> inputs = new() { pair.Phrase1, pair.Phrase2 };
                        // Generate embeddings for the input texts
                        OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

                        // Extract embeddings for each text
                        float[] embedding1 = collection[0].ToFloats().ToArray();
                        float[] embedding2 = collection[1].ToFloats().ToArray();

                        // Fix: Convert ReadOnlyMemory<float> to array before using LINQ methods
                        File.WriteAllLines("embedding_values.csv", collection[0].ToFloats().ToArray().Select(v => v.ToString()));
                        File.WriteAllLines("embedding_values1.csv", collection[1].ToFloats().ToArray().Select(v => v.ToString()));

                        // Print scalar values of the embeddings to the console
                        Console.WriteLine("Scalar values for text1:");
                       // PrintScalarValues(embedding1);

                        Console.WriteLine("Scalar values for text2:");
                        // PrintScalarValues(embedding2);



                        var similarity = CalculateSimilarity(embedding1, embedding2);
                        Console.WriteLine($"Embedding1 length: {embedding1.Length}, Embedding2 length: {embedding2.Length}");
                        Console.WriteLine($"Similarity: {similarity:F4}");
                        PhraseSimilarity obj2 = new PhraseSimilarity();
                        obj2.Phrase1 = pair.Phrase1;
                        obj2.Phrase2 = pair.Phrase2;
                        obj2.Domain = pair.Domain;
                        obj2.Context = pair.Context;
                        obj2.SimilarityScore = similarity;
                        obj.PhrasePairs.Add(obj2);
                        results.Add(obj2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                //pair.SimilarityScore = await GetSimilarityScore(pair.Phrase1, pair.Phrase2, huggingFaceApiUrl, apiToken);
            }
        }

        // Process document pairs
        //foreach (var pair in dataset.DocumentPairs)
        //{
        //    pair.SimilarityScore = await GetSimilarityScore(pair.Document1, pair.Document2, huggingFaceApiUrl, apiToken);
        //}
       // Console.WriteLine($"File: {sourceFile}, Domain: {phraseSimilarity.domain}");
       
        // Save results back to JSON
        string currentDir = Directory.GetCurrentDirectory();
        File.WriteAllText(currentDir + "\\data\\output_dataset.json", JsonConvert.SerializeObject(obj, Formatting.Indented));
        Console.WriteLine(currentDir + "Results saved to output_dataset.json.");

        string currentDir1 = Directory.GetCurrentDirectory();
        string outputPath = Path.Combine(currentDir1, "data", "output_datasetphrases.csv");
        using (var writer = new StreamWriter(outputPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(results);
        }
        Console.WriteLine($"Phrase Similarity Results saved to {outputPath}.");
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


    // Models for Dataset
    class InputDataset
    {
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new List<PhraseSimilarity>();

        //[JsonProperty("document_pairs")]
        //public List<DocumentSimilarity> DocumentPairs { get; set; }
    }

    class PhraseSimilarity
    {
        public string? Phrase1 { get; set; }
        public string? Phrase2 { get; set; }
        public string? Domain { get; set; }
        public string? Context { get; set; }
        public double SimilarityScore { get; set; }
    }

}
