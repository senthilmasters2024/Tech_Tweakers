using Microsoft.Extensions.DependencyInjection;
using OpenAI.Embeddings;
using SemanticAnalysisTextualData.Source.Interfaces;
using SemanticAnalysisTextualData.Source.pojo;
using SemanticAnalysisTextualData.Source.Utils;
using SemanticAnalysisTextualData.Util;

namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Provides methods for comparing the semantic similarity of documents using dynamic input data.
    /// </summary>
    public class SemanticSimilarityForDocumentsWithInputDataDynamic : ISimilarityService, IEmbedding
    {


        /// <summary>
        /// Invokes the document comparison process with the specified arguments.
        /// </summary>
        /// <param name="isPreProcessRequiredFlag">The arguments for the document comparison process.</param>
        public async Task InvokeDocumentComparsion(bool isPreProcessRequiredFlag)
        {
            var serviceProvider = ConfigureServices();
            var textAnalysisService = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();

            if (textAnalysisService != null)
            {
                try
                {
                    var (sourceFiles, targetFiles) = GetSourceAndTargetFiles(isPreProcessRequiredFlag);
                    var results = await CompareDocumentsAsync(sourceFiles, targetFiles);
                    CsvHelperUtil.SaveResultsToCsv(results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Configures the services required for the document comparison process.
        /// </summary>
        /// <returns>A ServiceProvider instance with the configured services.</returns>
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<SemanticSimilarityForDocumentsWithInputDataDynamic>(provider => new SemanticSimilarityForDocumentsWithInputDataDynamic());
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Gets the source and target files for the document comparison process.
        /// </summary>
        /// <returns>A tuple containing arrays of source and target file paths.</returns>
        public (string[] sourceFiles, string[] targetFiles) GetSourceAndTargetFiles(bool isPreProcessRequiredFlag)
        {
            // Get the project root directory
            string? projectRoot = AppContext.BaseDirectory;
            if (projectRoot == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }

            // Define the base data folder and subfolders for source and target files
            string baseDataFolder = Path.Combine(projectRoot, Constants.BaseDataFolder);
            string sourceFolder;
            string targetFolder;
            if (isPreProcessRequiredFlag)
            {
                sourceFolder = Path.Combine(baseDataFolder, Constants.ProcessedSourceFolder);
                targetFolder = Path.Combine(baseDataFolder, Constants.ProcessedTargetFolder);
            }
            else
            {
                sourceFolder = Path.Combine(baseDataFolder, Constants.SourceFolder);
                targetFolder = Path.Combine(baseDataFolder, Constants.TargetFolder);
            }

            // Get the list of source and target files
            string[] sourceFiles = Directory.GetFiles(sourceFolder, Constants.TextFileExtension);
            string[] targetFiles = Directory.GetFiles(targetFolder, Constants.TextFileExtension);

            return (sourceFiles, targetFiles);

        }

        /// <summary>
        /// Compares the documents asynchronously and returns a list of document similarities.
        /// </summary>
        /// <param name="sourceFiles">The source files to compare.</param>
        /// <param name="targetFiles">The target files to compare.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of document similarities.</returns>
        public async Task<List<DocumentSimilarity>> CompareDocumentsAsync(string[] sourceFiles, string[] targetFiles)
        {
            var results = new List<DocumentSimilarity>();
            foreach (var sourceFile in sourceFiles)
            {
                string fileName1 = Path.GetFileName(sourceFile);
                Console.WriteLine($"File Name: {fileName1}");

                // Read the content of the source file
                string sentence1 = await File.ReadAllTextAsync(sourceFile);

                foreach (var targetFile in targetFiles)
                {
                    string fileName2 = Path.GetFileName(targetFile);
                    Console.WriteLine($"File Name: {fileName2}");

                    // Read the content of the target file
                    string sentence2 = await File.ReadAllTextAsync(targetFile);

                    // Calculate the similarity between the source and target sentences
                    var similarity = await CalculateEmbeddingAsync(sentence1, sentence2, fileName1, fileName2);
                    Console.WriteLine($"Similarity between {fileName1} and {fileName2}: {similarity:F4}");

                    // Create a DocumentSimilarity object and add it to the results list
                    var phraseSimilarity = CreateDocumentSimilarity(fileName1, fileName2, similarity);
                    Console.WriteLine($"File: {sourceFile}, Domain: {phraseSimilarity.domain}");
                    results.Add(phraseSimilarity);
                }
            }
            return results;
        }

        /// <summary>
        /// Creates a DocumentSimilarity object based on the file names and similarity score.
        /// </summary>
        /// <param name="fileName1">The name of the first file.</param>
        /// <param name="fileName2">The name of the second file.</param>
        /// <param name="similarity">The similarity score between the two files.</param>
        /// <returns>A DocumentSimilarity object.</returns>
        public DocumentSimilarity CreateDocumentSimilarity(string fileName1, string fileName2, double similarity)
        {
            var phraseSimilarity = new DocumentSimilarity
            {
                FileName1 = fileName1,
                FileName2 = fileName2,
                SimilarityScore = similarity
            };

            // Determine the domain based on the file name
            if (fileName1.StartsWith("preprocessed_JobProfile", StringComparison.OrdinalIgnoreCase))
            {
                phraseSimilarity.domain = Constants.JobProfileDomain;
            }
            else if (fileName1.StartsWith("preprocessed_MedicalHistory", StringComparison.OrdinalIgnoreCase))
            {
                phraseSimilarity.domain = Constants.MedicalHistoryDomain;
            }
            else
            {
                phraseSimilarity.domain = Constants.UnknownDomain; // Default or fallback domain
            }

            return phraseSimilarity;
        }

        /// <summary>
        /// Calculates the embedding similarity between two texts asynchronously.
        /// </summary>
        /// <param name="text1">The first text to compare.</param>
        /// <param name="text2">The second text to compare.</param>
        /// <param name="fileName1">The name of the first file.</param>
        /// <param name="fileName2">The name of the second file.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score.</returns>
        public async Task<double> CalculateEmbeddingAsync(string text1, string text2, string fileName1, string fileName2)
        {
            try
            {
                // Initialize the embedding client with the API key
                EmbeddingClient client = new(Constants.EmbeddingModel, Environment.GetEnvironmentVariable(Constants.OpenAIAPIKeyEnvVar));
                List<string> inputs = new() { text1, text2 };

                // Generate embeddings for the input texts
                OpenAIEmbeddingCollection collection = await client.GenerateEmbeddingsAsync(inputs);

                // Convert the embeddings to float arrays
                float[] embedding1 = collection[0].ToFloats().ToArray();
                float[] embedding2 = collection[1].ToFloats().ToArray();

                // Save the embeddings to CSV files
                File.WriteAllLines(fileName1 + Constants.EmbeddingValuesSuffix, embedding1.Select(v => v.ToString()));
                File.WriteAllLines(fileName2 + Constants.EmbeddingValues1Suffix, embedding2.Select(v => v.ToString()));

                // Print scalar values for the embeddings
                Console.WriteLine("Scalar values for text1:");
                PrintScalarValues(embedding1);

                Console.WriteLine("Scalar values for text2:");
                PrintScalarValues(embedding2);

                // Calculate the similarity between the embeddings
                var similarity = CalculateSimilarity(embedding1, embedding2);
                Console.WriteLine($"Embedding1 length: {embedding1.Length}, Embedding2 length: {embedding2.Length}");

                return similarity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating similarity: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Prints each scalar value in the embedding.
        /// </summary>
        /// <param name="embedding">The embedding array containing scalar values.</param>
        public static void PrintScalarValues(float[] embedding)
        {
            // Print each scalar value in the embedding
            for (int i = 0; i < embedding.Length; i++)
            {
                Console.WriteLine($"Word {i + 1}: {embedding[i]}");
            }
        }

        /// <summary>
        /// Calculates the similarity between two embeddings.
        /// </summary>
        /// <param name="embedding1">The first embedding vector.</param>
        /// <param name="embedding2">The second embedding vector.</param>
        /// <returns>The similarity score between the two embeddings.</returns>
        public double CalculateSimilarity(float[] embedding1, float[] embedding2)
        {
            try
            {
                // Ensure the embeddings have the same length
                if (embedding1.Length != embedding2.Length)
                {
                    return 0;
                }

                double dotProduct = 0.0;
                double magnitude1 = 0.0;
                double magnitude2 = 0.0;

                // Calculate the dot product and magnitudes of the embeddings
                for (int i = 0; i < embedding1.Length; i++)
                {
                    dotProduct += embedding1[i] * embedding2[i];
                    magnitude1 += Math.Pow(embedding1[i], 2);
                    magnitude2 += Math.Pow(embedding2[i], 2);
                }

                magnitude1 = Math.Sqrt(magnitude1);
                magnitude2 = Math.Sqrt(magnitude2);

                // Ensure the magnitudes are not zero
                if (magnitude1 == 0.0 || magnitude2 == 0.0)
                {
                    throw new ArgumentException("Embedding vectors must not have zero magnitude.");
                }

                Console.WriteLine("Magnitude 1 and 2 is" + magnitude1 + "and" + magnitude2);

                // Calculate the cosine similarity
                double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);
                return cosineSimilarity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating cosine similarity: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Calculates the similarity between two texts asynchronously.
        /// </summary>
        /// <param name="text1">The first text to compare.</param>
        /// <param name="text2">The second text to compare.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score.</returns>
        public Task<double> CalculateSimilarityAsync(string text1, string text2)
        {
            throw new NotImplementedException();
        }
    }
}