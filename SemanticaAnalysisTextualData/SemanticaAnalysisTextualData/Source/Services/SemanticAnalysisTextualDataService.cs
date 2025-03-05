using OpenAI.Embeddings;
using ScottPlot.Palettes;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Interfaces;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TiktokenSharp;
using static App.Metrics.Health.HealthCheck;




namespace SemanticaAnalysisTextualData.Source.Services
{
    // Existing code...

    public class SemanticAnalysisTextualDataService : ISemanticAnalysisTextualDataInterface
    {
        public Task CalculateSimilarityForDocumentsAsync(string processedRequirementsFolder, string processedResumesFolder)
        {
            throw new NotImplementedException();
        }

        public Task CalculateSimilarityForWordsAndPhrasesAsync(List<double[]> wordEmbeddings, List<double[]> phraseEmbeddings)
        {
            throw new NotImplementedException();
        }

        // Existing code...

        // Cosine Similarity Function
        public double ComputeCosineSimilarity(double[] vectorA, double[] vectorB)
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

        public Task GenerateEmbeddingsForWordsAndPhrases(string wordsFolder, string phrasesFolder)
        {
            throw new NotImplementedException();
        }

        public Task PreprocessAllDocuments(string requirementsFolder, string resumesFolder, string outputRequirements, string outputResumes)
        {
            throw new NotImplementedException();
        }

        public Task PreprocessWordsAndPhrases(string wordsFolder, string phrasesFolder, string outputWords, string outputPhrases)
        {
            throw new NotImplementedException();
        }

        // Other methods...
    }
        // Cosine Similarity Function
        //public double ComputeCosineSimilarity1(double[] vectorA, double[] vectorB)
        //{
        //    if (vectorA.Length != vectorB.Length)
        //        throw new ArgumentException("Vectors must be of the same dimension.");

        //    double dotProduct = 0.0;
        //    double magnitudeA = 0.0;
        //    double magnitudeB = 0.0;

        //    for (int i = 0; i < vectorA.Length; i++)
        //    {
        //        dotProduct += vectorA[i] * vectorB[i];
        //        magnitudeA += Math.Pow(vectorA[i], 2);
        //        magnitudeB += Math.Pow(vectorB[i], 2);
        //    }

        //    magnitudeA = Math.Sqrt(magnitudeA);
        //    magnitudeB = Math.Sqrt(magnitudeB);

        //    return dotProduct / (magnitudeA * magnitudeB);
        //}

        //public Task CalculateSimilarityForWordsAndPhrasesAsync(List<double[]> wordEmbeddings, List<double[]> phraseEmbeddings)
        //{
        //    throw new NotImplementedException();
        //}
    }
//}
// Generate embeddings for words and phrases (useful if you want to calculate similarity between them)


// Calculate similarity for words and phrases
// async Task CalculateSimilarityForWordsAndPhrasesAsync(List<double[]> wordEmbeddings, List<double[]> phraseEmbeddings)
//{
//            await Task.Run(() =>
//            {
//                Console.WriteLine("Calculating similarity between words...");
//                Parallel.For(0, wordEmbeddings.Count, i =>
//                {
//                    for (int j = 0; j < wordEmbeddings.Count; j++)
//                    {
//                        if (i != j)
//                        {
//                            double similarity = ComputeCosineSimilarity(wordEmbeddings[i], wordEmbeddings[j]);
//                            Console.WriteLine($"Word {i + 1} vs Word {j + 1} | Similarity: {similarity}");
//                        }
//                    }
//                });

//                Console.WriteLine("Calculating similarity between phrases...");
//                Parallel.For(0, phraseEmbeddings.Count, i =>
//                {
//                    for (int j = 0; j < phraseEmbeddings.Count; j++)
//                    {
//                        if (i != j)
//                        {
//                            double similarity = ComputeCosineSimilarity(phraseEmbeddings[i], phraseEmbeddings[j]);
//                            Console.WriteLine($"Phrase {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
//                        }
//                    }
//                });

//                Console.WriteLine("Calculating similarity between words and phrases...");
//                Parallel.For(0, wordEmbeddings.Count, i =>
//                {
//                    for (int j = 0; j < phraseEmbeddings.Count; j++)
//                    {
//                        double similarity = ComputeCosineSimilarity(wordEmbeddings[i], phraseEmbeddings[j]);
//                        Console.WriteLine($"Word {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
//                    }
//                });
//            });

//}



/// <summary>
/// Loads embeddings from a file asynchronously and converts them into a list of double arrays.
/// </summary>
 //async Task<List<double[]>> LoadEmbeddingsFromFileAsync(string filePath)
 //       {
 //           var embeddings = new List<double[]>();

 //           if (File.Exists(filePath))
 //           {
 //               var lines = await File.ReadAllLinesAsync(filePath);
 //               foreach (var line in lines)
 //               {
 //                   double[] vector = line.Split(',')
 //                                         .Select(x => double.TryParse(x, out double value) ? value : 0.0)
 //                                         .ToArray();
 //                   embeddings.Add(vector);
 //               }
 //           }
 //           else
 //           {
 //               throw new FileNotFoundException($"Embedding file not found: {filePath}");
 //           }

 //           return embeddings;
 //       }

        // Other methods...
    
     