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
            _textPreprocessor.ProcessAndSaveDocuments(wordsFolder, outputWords);
            _textPreprocessor.ProcessAndSaveDocuments(phrasesFolder, outputPhrases);
            await Task.CompletedTask;
        }

        public void CalculateSimilarity(float[] vectorA, float[] vectorB)
        {
            // Your cosine similarity calculation logic here.
            double similarity = ComputeCosineSimilarity(
                 Array.ConvertAll(vectorA, x => (double)x),
                 Array.ConvertAll(vectorB, x => (double)x)
                 );
           
        }
    

        //Processes all documents in the specified directories before similarity calculations
        public async Task PreprocessAllDocuments(string requirementsFolder, string resumesFolder, string outputRequirements, string outputResumes)
        {

             _textPreprocessor.ProcessAndSaveDocuments(requirementsFolder, outputRequirements);


           _textPreprocessor.ProcessAndSaveDocuments(resumesFolder, outputResumes);

            return ;     
                
         }
        // Loads preprocessed text from files
        private List<string> LoadWordsFromFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.txt")
                           .Select(File.ReadAllText)
                           .ToList();
        }

        private List<string> LoadPhrasesFromFolder(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.txt")
                           .Select(File.ReadAllText)
                           .ToList();
        }

        private List<string> LoadPreprocessedDocuments(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.txt")
               .Select(File.ReadAllText)
               .ToList();
            
            //List<string> documents = new();
                          //foreach (var file in Directory.GetFiles(folderPath, "*.txt"))
                          //{
                          //documents.Add(File.ReadAllText(file));
                          //}
                          //return documents;
        }

        //Asynchronously calculates similarity between job descriptions and resumes
        public async Task CalculateSimilarityForDocumentsAsync(string processedRequirementsFolder, string processedResumesFolder)

        {

            // Console.WriteLine("Loading preprocessed documents...");
            var processedJobDescriptions = LoadPreprocessedDocuments(processedRequirementsFolder);
            var processedResumes = LoadPreprocessedDocuments(processedResumesFolder);

            if (!processedJobDescriptions.Any() || !processedResumes.Any())
            {
                throw new InvalidOperationException("No documents found in preprocessed folders.");
                //Console.WriteLine("No documents found in preprocessed folders.");
                //return;
            }
            var client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            var jobDescEmbeddings = await client.GenerateEmbeddingsAsync(processedJobDescriptions);
            var resumeEmbeddings = await client.GenerateEmbeddingsAsync(processedResumes);





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
                    double similarity = ComputeCosineSimilarity(
                        Array.ConvertAll(jobDescriptionEmbeddings[i], x => (float)x),
                Array.ConvertAll(resumeEmbeddingsList[j], x => (float)x)
                );

                    results.Add((i, j, similarity));


                    //(jobDescriptionEmbeddings[i], resumeEmbeddingsList[j])
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
        public async Task GenerateEmbeddingsForWordsAndPhrases(string wordsFolder, string phrasesFolder)
        {
            var client = new EmbeddingClient("text-embedding-3-large", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            // Load raw words and phrases
            var rawWords = LoadWordsFromFolder(wordsFolder);
            var rawPhrases = LoadPhrasesFromFolder(phrasesFolder);

            // Preprocess words and phrases
            var words = rawWords.Select(w => _textPreprocessor.PreprocessText(w, TextDataType.Word)).Where(w => !string.IsNullOrEmpty(w)).ToList();
            var phrases = rawPhrases.Select(p => _textPreprocessor.PreprocessText(p, TextDataType.Phrase)).Where(p => !string.IsNullOrEmpty(p)).ToList();

            // Generate embeddings for words and phrases
            var wordEmbeddings = await client.GenerateEmbeddingsAsync(words);
            var phraseEmbeddings = await client.GenerateEmbeddingsAsync(phrases);

            // Convert embeddings to double arrays
            var wordEmbeddingsList = wordEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();
            var phraseEmbeddingsList = phraseEmbeddings.Value.Select(e => e.Vector.Select(x => (double)x).ToArray()).ToList();

            // Now you have embeddings for words and phrases, ready to calculate similarity
            CalculateSimilarityForWordsAndPhrases(wordEmbeddingsList, phraseEmbeddingsList);
        }

        // Calculate similarity for words and phrases
        public void CalculateSimilarityForWordsAndPhrases(List<double[]> wordEmbeddingsList, List<double[]> phraseEmbeddingsList)
        {
            Console.WriteLine("Calculating similarity between words...");
            for (int i = 0; i < wordEmbeddingsList.Count; i++)
            {
                for (int j = 0; j < wordEmbeddingsList.Count; j++)
                {
                    if (i != j)
                    {
                        double similarity = ComputeCosineSimilarity(wordEmbeddingsList[i], wordEmbeddingsList[j]);
                        Console.WriteLine($"Word {i + 1} vs Word {j + 1} | Similarity: {similarity}");
                    }
                }
            }

            Console.WriteLine("Calculating similarity between phrases...");
            for (int i = 0; i < phraseEmbeddingsList.Count; i++)
            {
                for (int j = 0; j < phraseEmbeddingsList.Count; j++)
                {
                    if (i != j)
                    {
                        double similarity = ComputeCosineSimilarity(phraseEmbeddingsList[i], phraseEmbeddingsList[j]);
                        Console.WriteLine($"Phrase {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
                    }
                }
            }

            Console.WriteLine("Calculating similarity between words and phrases...");
            for (int i = 0; i < wordEmbeddingsList.Count; i++)
            {
                for (int j = 0; j < phraseEmbeddingsList.Count; j++)
                {
                    double similarity = ComputeCosineSimilarity(wordEmbeddingsList[i], phraseEmbeddingsList[j]);
                    Console.WriteLine($"Word {i + 1} vs Phrase {j + 1} | Similarity: {similarity}");
                }
            }
        }
    }
}

  







/* private string PreprocessText(string text1, string text2)
 {
     // Preprocess at word level
     var wordProcessedText = _wordPreprocessor.ProcessWords(text1, text2);

     // Preprocess at sentence level
     var sentenceProcessedText = _sentencePreprocessor.ProcessSentences(text1, text2);

     // Preprocess at document level
     var documentProcessedText = _documentPreprocessor.ProcessTwoDocuments(text1, text2);

     // Combine all preprocessed data (you can customize this logic based on your needs)
     return $"{wordProcessedText} | {sentenceProcessedText} | {documentProcessedText}";
 }

 // { Create an EmbeddingClient instance using the OpenAI API key
 //EmbeddingClient client = new("text-embedding-3-large" /* Optional: Replace with "text-embedding-3-small"  
 //  Environment.GetEnvironmentVariable("OPENAI_API_KEY"));


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
 //List<string> inputs = new() { text1, text2 };

 // Generate embeddings for the input texts
 //OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

 //Sample Embedded Vales for Fun
 //float[] fun = [0.25f, 0.85f,-0.12f, 0.56f, 0.47f];
 //// Calculate similarity between the two embeddings
 ////Sample Embedded Vales for Fun
 //float[] joy = [0.27f, 0.81f, -0.10f, 0.60f, 0.50f];
 // var similarity = CalculateSimilarity(
 // collection[0].ToFloats().ToArray(), collection[1].ToFloats().ToArray()
 //);
 //return similarity;
 //}



 /// <summary>
 /// Calculates the cosine similarity between two embeddings.
 /// </summary>
 /// <param name="embedding1">The first embedding vector as an array of floats.</param>
 /// <param name="embedding2">The second embedding vector as an array of floats.</param>
 /// <returns>The cosine similarity score between the two embeddings.</returns>
/// <exception cref="ArgumentException">Thrown when the embeddings have different lengths or zero magnitude.</exception>


//Calculates the cosine similarity between two embeddings.

public double CalculateSimilarity(float[] embedding1, float[] embedding2);





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

return (magnitude1 == 0.0 || magnitude2 == 0.0) ? 0 : dotProduct / (magnitude1 * magnitude2);






/* DONT DELETE
// Check for zero magnitude
     if (magnitude1 == 0.0 || magnitude2 == 0.0)
     {
         throw new ArgumentException("Embedding vectors must not have zero magnitude.");
     }

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


DELETED PORTION //  Generate embeddings for each job description and resume

            foreach (var jobDescription in processedJobDescriptions)
            {
                foreach (var resume in processedResumes)
                {
                    List<string> inputs = new() { jobDescription, resume };
                    OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);
                    var jobDescriptionEmbedding = collection[0].ToFloats().ToArray();
                    var resumeEmbedding = collection[1].ToFloats().ToArray();



                    // Compute similarity
                    double similarity = ComputeCosineSimilarity(jobDescriptionEmbedding, resumeEmbedding);

                    Console.WriteLine($"Similarity between Job Description and Resume: {similarity}");


                }


            }
        }

*/
