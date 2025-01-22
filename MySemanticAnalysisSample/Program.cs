using MySemanticAnalysisSample.Embedding;
using MySemanticAnalysisSample.FileHandling;
using MySemanticAnalysisSample.SimilarityCalculation;

namespace MySemanticAnalysisSample
{
    internal class Program
    {
        //--------------------Program to take 2 inputs from console and calculate similarity--------------------------------------------------
        //static async Task Main(string[] args)
        //{
        //    Console.WriteLine("Welcome to semantic analysis of text data");
        //    Console.WriteLine("Enter first text");
        //    var text1 = Console.ReadLine();
        //    Console.WriteLine("Enter second text");
        //    var text2 = Console.ReadLine();

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrEmpty(text2))
        //        {
        //            throw new ArgumentException("Input text cannot be null or whitespace.");
        //        }

        //        var embeddingGenerator = new ChatGPTEmbeddingGenerator();

        //        // Generate the embedding
        //        var embedding1 = await embeddingGenerator.CreateEmbedding(text1);
        //        var embedding2 = await embeddingGenerator.CreateEmbedding(text2);

        //        // Display embedding details
        //        //Console.WriteLine("Embedding generated successfully!");

        //        // Display the first 10 values for clarity
        //        Console.WriteLine("First 10 values of the embedding1:");
        //        Console.WriteLine(string.Join(", ", embedding1.Take(10)));

        //        Console.WriteLine("First 10 values of the embedding2:");
        //        Console.WriteLine(string.Join(", ", embedding2.Take(10)));

        //        var similarityCalculator = new CosineSimilarityCalculator();
        //        var similarity = similarityCalculator.CalculateSimilarity(embedding1, embedding2);
        //        Console.WriteLine("Similarity score is " + similarity);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}

        //---------program to read data from file and calculate similarity---------------------------------------------
        static async Task Main(string[] args)
        {
            string inputTextPath = @"C:\Users\NISHTA\OneDrive\Univeristy\sem_1\software_eng\ML_09\MySemanticAnalysisSample\Input\InputText\";
            string domainsPath = @"C:\Users\NISHTA\OneDrive\Univeristy\sem_1\software_eng\ML_09\MySemanticAnalysisSample\Input\Domains\";

            if (!Directory.Exists(inputTextPath) || !Directory.Exists(domainsPath))
            {
                Console.WriteLine("The specified path does not exist");
                return;
            }

            var domainReader = new FileReader(domainsPath);
            var domains = domainReader.ReadDocuments();
            var inputTextReader = new FileReader(inputTextPath);
            var inputTexts = inputTextReader.ReadDocuments();

            try
            {
                if (domains == null || inputTexts == null)
                {
                    throw new NullReferenceException();  //add proper exception
                }

                var domainEmbeddingDictionary = new Dictionary<string, float[]>();
                var embeddingGenerator = new ChatGPTEmbeddingGenerator();

                foreach (var domain in domains)
                {
                    //preprocess
                    //check for duplicate domain keys
                    var embedding = await embeddingGenerator.CreateEmbedding(domain.Value);
                    domainEmbeddingDictionary.Add(domain.Key, embedding);
                }
                var similarityCalculator = new CosineSimilarityCalculator();

                foreach (var inputText in inputTexts)
                {
                    //preprocess
                    var embedding = await embeddingGenerator.CreateEmbedding(inputText.Value);

                    foreach (var domainEmbedding in domainEmbeddingDictionary)
                    {
                        var similarity = similarityCalculator.CalculateSimilarity(embedding, domainEmbedding.Value);
                        Console.WriteLine(inputText.Key + "," + domainEmbedding.Key + "->" + similarity);  //write to csv

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
