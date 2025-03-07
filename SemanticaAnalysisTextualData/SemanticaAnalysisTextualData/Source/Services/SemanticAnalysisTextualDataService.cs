
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

    public class SemanticAnalysisTextualDataService : ISimilarityService
    {


        //Create a constructor If needed to Initialise Anything

        /// <summary>
        /// Asynchronous method to generate embeddings for two text inputs and calculate their similarity.
        /// </summary>
        private readonly IPreprocessor _preprocessor;

        public SemanticAnalysisTextualDataService(IPreprocessor preprocessor)
        {
            _preprocessor = preprocessor;
        }
        public async Task PreprocessWordsAndPhrases(string wordsFolder, string phrasesFolder, string outputWords, string outputPhrases)
        {
            await _preprocessor.ProcessAndSaveWordsAsync("Technology", wordsFolder, outputWords);//Edit this to make dynamic. Remove this Hardcoding
            await _preprocessor.ProcessAndSavePhrasesAsync("Sports", phrasesFolder, outputPhrases);


            // Create an EmbeddingClient instance using the OpenAI API key
            //EmbeddingClient client = new("text-embedding-3-large" /* Optional: Replace with "text-embedding-3-small" */


            // Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        }


        // Infinite loop to allow repeated similarity calculations
        //while (true)
        //{
        // Prompt the user to input the first text
        //Console.WriteLine("Enter text 1: ");
        //var inp1 = Console.ReadLine();

        //// Prompt the user to input the second text
        //Console.WriteLine("Enter text 2: ");
        //var inp2 = Console.ReadLine();

        // Validate user input
        //if (string.IsNullOrWhiteSpace(inp1) || string.IsNullOrWhiteSpace(inp2))
        //{
        //    Console.WriteLine("Both inputs must be non-empty. Please try again.");
        //    continue;
        //}

        // Prepare the inputs for embedding generation


        // List<string> inputs = new() { text1, text2 };

        // Generate embeddings for the input texts

        // OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);
        // Extract all embedding vectors as float arrays
        // List<float[]> allEmbeddings = collection.Select(embedding => embedding.ToFloats().ToArray()).ToList();

        //Sample Embedded Vales for Fun
        //float[] fun = [0.25f, 0.85f,-0.12f, 0.56f, 0.47f];
        //// Calculate similarity between the two embeddings
        ////Sample Embedded Vales for Fun
        //float[] joy = [0.27f, 0.81f, -0.10f, 0.60f, 0.50f];

        // var similarity = ComputeCosineSimilarity(
        //  collection[0].ToFloats().ToArray().Select(x => (double)x).ToArray(),
        //collection[1].ToFloats().ToArray().Select(x => (double)x).ToArray()

        // );
        //Console.WriteLine($"Embedding1 length: {collection[0].ToFloats().ToArray().Length}, Embedding2 length: {collection[1].ToFloats().ToArray().Length}");
        //return similarity;
        //}


        //public void CalculateSimilarity(float[] vectorA, float[] vectorB)
        // {
        // Your cosine similarity calculation logic here.
        // double similarity = ComputeCosineSimilarity(
        //  Array.ConvertAll(vectorA, x => (double)x),
        // Array.ConvertAll(vectorB, x => (double)x)
        // );

        // }


        //Processes all documents in the specified directories before similarity calculations
        public async Task PreprocessAllDocuments(string requirementsFolder, string resumesFolder, string outputRequirements, string outputResumes)
        {

            await _preprocessor.ProcessAndSaveDocumentsAsync("Technolgy", requirementsFolder, outputRequirements);


            await _preprocessor.ProcessAndSaveDocumentsAsync("Peter", resumesFolder, outputResumes);

            return;

        }
        // Loads preprocessed text from files
        private List<string> LoadTextFilesFromFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            if (!files.Any())
            {
                Console.WriteLine($" No text files found in {folderPath}");
                return new List<string>();  // Return empty list instead of throwing an error
                //throw new InvalidOperationException($"No text files found in {folderPath}");
            }
            // return files.Select(File.ReadAllText).ToList();
            Console.WriteLine($" Found {files.Length} text files in {folderPath}");

            List<string> fileContents = new List<string>();

            foreach (var file in files)
            {
                Console.WriteLine($"Reading file: {file}");
                string content = File.ReadAllText(file);
                Console.WriteLine($" File Content (first 100 chars): {content.Substring(0, Math.Min(content.Length, 100))}...");
                fileContents.Add(content);
            }

            return fileContents;

        }

        public async Task GenerateEmbeddingsForWordsAndPhrases(string wordsFolder, string phrasesFolder)
        {
            var client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            // Load raw words and phrases
            var rawWords = LoadTextFilesFromFolder(wordsFolder);
            var rawPhrases = LoadTextFilesFromFolder(phrasesFolder);

            // Preprocess words and phrases
            var words = rawWords.Select(w => _preprocessor.PreprocessText(w, TextDataType.Word)).Where(w => !string.IsNullOrEmpty(w)).ToList();
            var phrases = rawPhrases.Select(p => _preprocessor.PreprocessText(p, TextDataType.Phrase)).Where(p => !string.IsNullOrEmpty(p)).ToList();

            // Generate embeddings for words and phrases
            var wordEmbeddings = await client.GenerateEmbeddingsAsync(words);
            var phraseEmbeddings = await client.GenerateEmbeddingsAsync(phrases);


            // Convert embeddings to double arrays
            var wordEmbeddingsList = wordEmbeddings.Value.Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();

            var phraseEmbeddingsList = phraseEmbeddings.Value.Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();


            // Calculate similarity
            //await CalculateSimilarityForWordsAndPhrasesAsync(wordEmbeddingsList, phraseEmbeddingsList);
        }

        //Asynchronously calculates similarity between job descriptions and resumes
        public async Task CalculateSimilarityForDocumentsAsync(string processedRequirementsFolder, string processedResumesFolder)

        {

            // Console.WriteLine("Loading preprocessed documents...");
            var jobDescriptions = LoadTextFilesFromFolder(processedRequirementsFolder);
            var resumes = LoadTextFilesFromFolder(processedResumesFolder);

            var client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            var jobDescEmbeddings = await client.GenerateEmbeddingsAsync(jobDescriptions);
            var resumeEmbeddings = await client.GenerateEmbeddingsAsync(resumes);


            Console.WriteLine("Job Description Embeddings Response:");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(jobDescEmbeddings, Newtonsoft.Json.Formatting.Indented));

            Console.WriteLine("Resume Embeddings Response:");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(resumeEmbeddings, Newtonsoft.Json.Formatting.Indented));


            //Step 1: Generate embeddings for job descriptions

            // Console.WriteLine("Generating embeddings for job descriptions...");
            //List<string> jobDescInputs = new List<string>(processedJobDescriptions);
            //OpenAIEmbeddingCollection jobDescEmbeddings = await client.GenerateEmbeddingsAsync(processedJobDescriptions);

            //// Convert embeddings to double arrays
            var jobDescriptionEmbeddings = jobDescEmbeddings.Value.Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();




            // Step 2: Generate embeddings for resumes

            // Console.WriteLine("Generating embeddings for resumes...");
            //List<string> resumeInputs = new List<string>(processedResumes);
            //OpenAIEmbeddingCollection resumeEmbeddings = await client.GenerateEmbeddingsAsync(processedResumes);
            var resumeEmbeddingsList = resumeEmbeddings.Value.Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();
        }






            // Step 3: Compute similarity between each job description and resume
            /*
            Console.WriteLine("Computing similarity scores...");
            List<(int jobIndex, int resumeIndex, double similarity)> results = new();
            for (int i = 0; i < jobDescriptionEmbeddings.Count(); i++)
            {
                for (int j = 0; j < resumeEmbeddingsList.Count(); j++)
                {
                   // double similarity = ComputeCosineSimilarity(jobDescriptionEmbeddings[i], resumeEmbeddingsList[j]);
                   // results.Add((i, j, similarity));
                }
            }
        */


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
    }
}






        /// <summary>
        /// Loads embeddings from a file asynchronously and converts them into a list of double arrays.
        /// </summary>
        //private async Task<List<double[]>> LoadEmbeddingsFromFileAsync(string filePath);


        // Other methods...


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



