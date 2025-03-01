using Microsoft.Extensions.DependencyInjection;
using OpenAI.Embeddings;
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
    /// <summary>
    /// Service to Implement all our TextualDataSemantic Analsysi Implementations
    /// </summary>
    public class SemanticAnalysisTextualDataService : ISemanticAnalysisTextualDataInterface
    {

        //Create a constructor If needed to Initialise Anything

        /// <summary>
        /// Asynchronous method to generate embeddings for two text inputs and calculate their similarity.
        /// </summary>
        private readonly ITextPreprocessor _textPreprocessor;

        public SemanticAnalysisTextualDataService(ITextPreprocessor textPreprocessor)
        {
            _textPreprocessor = textPreprocessor;

        }
        public async Task PreprocessWordsAndPhrases(string wordsFolder, string phrasesFolder, string outputWords, string outputPhrases)
        {
            await _textPreprocessor.ProcessAndSaveDocuments(wordsFolder, outputWords);
            await _textPreprocessor.ProcessAndSaveDocuments(phrasesFolder, outputPhrases);
        }

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

            await _textPreprocessor.ProcessAndSaveDocuments(requirementsFolder, outputRequirements);


            await _textPreprocessor.ProcessAndSaveDocuments(resumesFolder, outputResumes);

            return;

        }
        // Loads preprocessed text from files
        private List<string> LoadTextFilesFromFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            if (!files.Any())
            {
                throw new InvalidOperationException($"No text files found in {folderPath}");
            }
            return files.Select(File.ReadAllText).ToList();
        }

        public async Task GenerateEmbeddingsForWordsAndPhrases(string wordsFolder, string phrasesFolder)
        {
            var client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            // Load raw words and phrases
            var rawWords = LoadTextFilesFromFolder(wordsFolder);
            var rawPhrases = LoadTextFilesFromFolder(phrasesFolder);

            // Preprocess words and phrases
            var words = rawWords.Select(w => _textPreprocessor.PreprocessText(w, TextDataType.Word)).Where(w => !string.IsNullOrEmpty(w)).ToList();
            var phrases = rawPhrases.Select(p => _textPreprocessor.PreprocessText(p, TextDataType.Phrase)).Where(p => !string.IsNullOrEmpty(p)).ToList();

            // Generate embeddings for words and phrases
            var wordEmbeddings = await client.GenerateEmbeddingsAsync(words);
            var phraseEmbeddings = await client.GenerateEmbeddingsAsync(phrases);

            // Convert embeddings to double arrays
            var wordEmbeddingsList = wordEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();
            var phraseEmbeddingsList = phraseEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();

            // Calculate similarity
            CalculateSimilarityForWordsAndPhrasesAsync(wordEmbeddingsList, phraseEmbeddingsList);
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





            //Step 1: Generate embeddings for job descriptions

            // Console.WriteLine("Generating embeddings for job descriptions...");
            //List<string> jobDescInputs = new List<string>(processedJobDescriptions);
            //OpenAIEmbeddingCollection jobDescEmbeddings = await client.GenerateEmbeddingsAsync(processedJobDescriptions);

            //// Convert embeddings to double arrays
            var jobDescriptionEmbeddings = jobDescEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();




            // Step 2: Generate embeddings for resumes

            // Console.WriteLine("Generating embeddings for resumes...");
            //List<string> resumeInputs = new List<string>(processedResumes);
            //OpenAIEmbeddingCollection resumeEmbeddings = await client.GenerateEmbeddingsAsync(processedResumes);
            var resumeEmbeddingsList = resumeEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();






            // Step 3: Compute similarity between each job description and resume

            Console.WriteLine("Computing similarity scores...");
            List<(int jobIndex, int resumeIndex, double similarity)> results = new();
            for (int i = 0; i < jobDescriptionEmbeddings.Count(); i++)
            {
                for (int j = 0; j < resumeEmbeddingsList.Count(); j++)
                {
                    double similarity = ComputeCosineSimilarity(jobDescriptionEmbeddings[i], resumeEmbeddingsList[j]);
                    results.Add((i, j, similarity));
                }
            }
        }




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
        // Generate embeddings for words and phrases (useful if you want to calculate similarity between them)


        // Calculate similarity for words and phrases
        public async Task CalculateSimilarityForWordsAndPhrasesAsync(string outputWords, string outputPhrases)
        {


            // Load embeddings asynchronously
            List<double[]> wordEmbeddingsList = await LoadEmbeddingsFromFileAsync(outputWords);
            List<double[]> phraseEmbeddingsList = await LoadEmbeddingsFromFileAsync(outputPhrases);

            await Task.Run(async () =>

            {
                Console.WriteLine("Calculating similarity between words...");
                Parallel.For(0, wordEmbeddingsList.Count, i =>
                {

                    for (int j = 0; j < wordEmbeddingsList.Count; j++)
                    {
                        if (i != j)
                        {
                            double similarity = ComputeCosineSimilarity(wordEmbeddingsList[i], wordEmbeddingsList[j]);
                            Console.WriteLine($"Word {i + 1} vs Word {j + 1} | Similarity: {similarity}");
                        }
                    }
                });
                Console.WriteLine("Calculating similarity between phrases...");
                Parallel.For(0, phraseEmbeddingsList.Count, i =>
                {
                    for (int j = 0; j < phraseEmbeddingsList.Count; j++)
                    {
                        if (i != j)
                        {
                            double similarity = ComputeCosineSimilarity(phraseEmbeddingsList[i], phraseEmbeddingsList[j]);
                            Console.WriteLine($"Word {i + 1} vs Word {j + 1} | Similarity: {similarity}");
                        }
                    }
                });

                Console.WriteLine("Calculating similarity between words and phrases...");
                Parallel.For(0, wordEmbeddingsList.Count, i =>
                {
                    for (int j = 0; j < phraseEmbeddingsList.Count; j++)
                    {

                        {
                            double similarity = ComputeCosineSimilarity(phraseEmbeddingsList[i], phraseEmbeddingsList[j]);
                            Console.WriteLine($"Phrase {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
                        }
                    }
                });

            });
        }


            /// <summary>
            /// Loads embeddings from a file asynchronously and converts them into a list of double arrays.
            /// </summary>
        private async Task<List<double[]>> LoadEmbeddingsFromFileAsync(string filePath)
        {
            var embeddings = new List<double[]>();

            if (File.Exists(filePath))
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                foreach (var line in lines)
                {
                    double[] vector = line.Split(',')
                                          .Select(x => double.TryParse(x, out double value) ? value : 0.0)
                                          .ToArray();
                    embeddings.Add(vector);
                }
            }
            else
            {
                throw new FileNotFoundException($"Embedding file not found: {filePath}");
            }

            return embeddings;
        }

        // Other methods...
    }
}
        
    



  






