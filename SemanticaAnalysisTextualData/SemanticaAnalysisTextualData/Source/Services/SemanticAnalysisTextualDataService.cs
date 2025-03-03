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
    /// <summary>
    /// Service to Implement all our TextualDataSemantic Analsysi Implementations
    /// </summary>
    public class SemanticAnalysisTextualDataService : ISemanticAnalysisTextualDataInterface
    {
        // Create a constructor if needed to initialize anything
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
            var words = rawWords.Select(w => _textPreprocessor.PreprocessText(w, TextDataType.Word)).Where(w => !string.IsNullOrEmpty(w)).ToList();
            var phrases = rawPhrases.Select(p => _textPreprocessor.PreprocessText(p, TextDataType.Phrase)).Where(p => !string.IsNullOrEmpty(p)).ToList();

            // Generate embeddings for words and phrases
            var wordEmbeddings = await client.GenerateEmbeddingsAsync(words);
            var phraseEmbeddings = await client.GenerateEmbeddingsAsync(phrases);


            // Convert embeddings to double arrays
            var wordEmbeddingsList = wordEmbeddings.Value .Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();

            var phraseEmbeddingsList = phraseEmbeddings.Value .Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()) .ToList();


            // Calculate similarity
           await CalculateSimilarityForWordsAndPhrasesAsync(wordEmbeddingsList, phraseEmbeddingsList);
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
            var resumeEmbeddingsList = resumeEmbeddings.Value .Select(e => e.ToFloats().ToArray().Select(x => (double)x).ToArray()).ToList();






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

            Console.WriteLine("Magnitude 1 and 2 is" + magnitude1 + "and" + magnitude2);
            // Compute cosine similarity
            double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);



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
        // Generate embeddings for words and phrases (useful if you want to calculate similarity between them)


        // Calculate similarity for words and phrases
        public async Task CalculateSimilarityForWordsAndPhrasesAsync(List<double[]> wordEmbeddings, List<double[]> phraseEmbeddings)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Calculating similarity between words...");
                Parallel.For(0, wordEmbeddings.Count, i =>
                {
                    for (int j = 0; j < wordEmbeddings.Count; j++)
                    {
                        if (i != j)
                        {
                            double similarity = ComputeCosineSimilarity(wordEmbeddings[i], wordEmbeddings[j]);
                            Console.WriteLine($"Word {i + 1} vs Word {j + 1} | Similarity: {similarity}");
                        }
                    }
                });

                Console.WriteLine("Calculating similarity between phrases...");
                Parallel.For(0, phraseEmbeddings.Count, i =>
                {
                    for (int j = 0; j < phraseEmbeddings.Count; j++)
                    {
                        if (i != j)
                        {
                            double similarity = ComputeCosineSimilarity(phraseEmbeddings[i], phraseEmbeddings[j]);
                            Console.WriteLine($"Phrase {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
                        }
                    }
                });

                Console.WriteLine("Calculating similarity between words and phrases...");
                Parallel.For(0, wordEmbeddings.Count, i =>
                {
                    for (int j = 0; j < phraseEmbeddings.Count; j++)
                    {
                        double similarity = ComputeCosineSimilarity(wordEmbeddings[i], phraseEmbeddings[j]);
                        Console.WriteLine($"Word {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
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
     