using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenAI.Embeddings;
using SemanticAnalysisTextualData.Source.Interfaces;
using SemanticAnalysisTextualData.Source.pojo;
using SemanticAnalysisTextualData.Source.Services;
using SemanticAnalysisTextualData.Source.Utils;
using SemanticAnalysisTextualData.Util;
using System.Linq;

namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Service class for computing semantic similarity between documents by using OpenAI embeddings.
    /// Supports dynamic input data and chunk-based processing.
    /// </summary>
    public class SemanticSimilarityForDocumentsWithInputDataDynamic : ISimilarityService, IEmbedding
    {
        private readonly ILogger<SemanticSimilarityForDocumentsWithInputDataDynamic> _logger;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public SemanticSimilarityForDocumentsWithInputDataDynamic(ILogger<SemanticSimilarityForDocumentsWithInputDataDynamic> logger)
        {
            _logger = logger;
        }

        public SemanticSimilarityForDocumentsWithInputDataDynamic()
        {
        }

        /// <summary>
        /// Entry point for invoking document similarity processing.
        /// </summary>
        /// <param name="isPreProcessRequiredFlag">Flag indicating whether to use preprocessed folders or raw input folders.</param>
        public async Task InvokeDocumentComparsion(bool isPreProcessRequiredFlag)
        {
            var serviceProvider = ConfigureServices();
            var textAnalysisService = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();

            if (textAnalysisService != null)
            {
                try
                {

                    
                    var (sourceFiles, targetFiles) = GetSourceAndTargetFiles(isPreProcessRequiredFlag);
                    await LoadTrainingDataAsync("C:\\MyWork\\Tech_Tweakers\\SemanticAnalysisTextualData\\SemanticAnalysisTextualData\\data\\SourceBasedOnNeededRelevance");
                    var results = await CompareDocumentsAsync(sourceFiles, targetFiles);
                    CsvHelperUtil.SaveResultsToCsv(results);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during document comparison");
                }
            }
        }

        /// <summary>
        /// Configures dependency injection services including logging.
        /// </summary>
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(config =>
            {
           
                config.SetMinimumLevel(LogLevel.Information);
            });
            services.AddSingleton<SemanticSimilarityForDocumentsWithInputDataDynamic>();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Retrieves source and target file mappings from directory structure.
        /// </summary>
        public (Dictionary<string, string[]> sourceMap, Dictionary<string, string[]> targetMap) GetSourceAndTargetFiles(bool isPreProcessRequiredFlag)
        {
            string? projectRoot = AppContext.BaseDirectory;
            if (projectRoot == null)
                throw new InvalidOperationException("Unable to determine the project root directory.");

            string baseDataFolder = Path.Combine(projectRoot, Constants.BaseDataFolder);
            string sourceRoot = Path.Combine(baseDataFolder, isPreProcessRequiredFlag ? Constants.ProcessedSourceFolder : Constants.SourceFolder);
            string targetRoot = Path.Combine(baseDataFolder, isPreProcessRequiredFlag ? Constants.ProcessedTargetFolder : Constants.TargetFolder);

            var sourceMap = Directory.GetDirectories(sourceRoot)
                .ToDictionary(
                    dir => Path.GetFileName(dir),
                    dir => Directory.GetFiles(dir, "*.*")
                        .Where(f => f.EndsWith(".txt") || f.EndsWith(".pdf"))
                        .ToArray());

            var targetMap = Directory.GetDirectories(targetRoot)
                .ToDictionary(
                    dir => Path.GetFileName(dir),
                    dir => Directory.GetFiles(dir, "*.*")
                        .Where(f => f.EndsWith(".txt") || f.EndsWith(".pdf"))
                        .ToArray());

            return (sourceMap, targetMap);
        }

        /// <summary>
        /// Compares documents in corresponding source and target folders asynchronously.
        /// </summary>
        public async Task<List<DocumentSimilarity>> CompareDocumentsAsync(Dictionary<string, string[]> sourceMap, Dictionary<string, string[]> targetMap)
        {
            var results = new List<DocumentSimilarity>();

            foreach (var category in sourceMap.Keys)
            {
                if (!targetMap.ContainsKey(category))
                {
                    _logger.LogWarning("No matching target subfolder for category: {Category}", category);
                    continue;
                }

                var sourceFiles = sourceMap[category];
                var targetFiles = targetMap[category];

                foreach (var sourceFile in sourceFiles)
                {
                    string sentence1 = await ReadTextContentAsync(sourceFile);
                    string fileName1 = Path.GetFileName(sourceFile);

                    foreach (var targetFile in targetFiles)
                    {
                        string sentence2 = await ReadTextContentAsync(targetFile);
                        //string sentence2 = await File.ReadAllTextAsync(targetFile);
                        string fileName2 = Path.GetFileName(targetFile);

                        float[] embedding1 = await GetAveragedEmbeddingAsync(sentence1, fileName1, Constants.EmbeddingValuesSuffix);
                        float[] embedding2 = await GetAveragedEmbeddingAsync(sentence2, fileName2, Constants.EmbeddingValues1Suffix);

                        var similarity = CalculateSimilarity(embedding1, embedding2);
                        var similarityResult = CreateDocumentSimilarity(fileName1, fileName2, similarity, category);
                        results.Add(similarityResult);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Constructs a DocumentSimilarity object with similarity score and domain category.
        /// </summary>
        public DocumentSimilarity CreateDocumentSimilarity(string fileName1, string fileName2, double similarity, string domain)
        {
            return new DocumentSimilarity
            {
                FileName1 = fileName1,
                FileName2 = fileName2,
                SimilarityScore = similarity,
                domain = domain
            };
        }

        /// <summary>
        /// Computes averaged embedding from chunked text using OpenAI client.
        /// </summary>
        private async Task<float[]> GetAveragedEmbeddingAsync(string text, string fileName, string suffix)
        {
            const int chunkSize = 3000;
            var client = new EmbeddingClient(Constants.EmbeddingModel, Environment.GetEnvironmentVariable(Constants.OpenAIAPIKeyEnvVar));
            var chunks = ChunkText(text, chunkSize);
            var embeddings = new List<float[]>();

            foreach (var chunk in chunks)
            {
                try
                {
                    OpenAIEmbeddingCollection response = await client.GenerateEmbeddingsAsync(new List<string> { chunk });
                    if (response.Count > 0)
                    {
                        float[] vector = response[0].ToFloats().ToArray();
                        embeddings.Add(vector);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to get embedding for a chunk in file: {FileName}", fileName);
                }
            }

            if (embeddings.Count == 0)
            {
                _logger.LogWarning("No embeddings generated for file: {FileName}", fileName);
                return Array.Empty<float>();
            }

            float[] averaged = AverageVectors(embeddings);
            SaveEmbedding(fileName, averaged, suffix);
            return averaged;
        }

        /// <summary>
        /// Writes the embedding vector to a file.
        /// </summary>
        private void SaveEmbedding(string fileName, float[] embedding, string suffix)
        {
            try
            {
                File.WriteAllLines(fileName + suffix, embedding.Select(v => v.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save embedding for file: {FileName}", fileName);
            }
        }

        /// <summary>
        /// Splits a large text into smaller chunks of defined character size.
        /// </summary>
        private List<string> ChunkText(string text, int chunkSize)
        {
            var chunks = new List<string>();
            for (int i = 0; i < text.Length; i += chunkSize)
            {
                chunks.Add(text.Substring(i, Math.Min(chunkSize, text.Length - i)));
            }
            return chunks;
        }

        /// <summary>
        /// Averages a list of embedding vectors into a single vector.
        /// </summary>
        private float[] AverageVectors(List<float[]> vectors)
        {
            int length = vectors[0].Length;
            float[] average = new float[length];

            foreach (var vec in vectors)
            {
                for (int i = 0; i < length; i++)
                {
                    average[i] += vec[i];
                }
            }

            for (int i = 0; i < length; i++)
            {
                average[i] /= vectors.Count;
            }

            return average;
        }

        /// <summary>
        /// Logs the first 10 scalar values from an embedding for debugging.
        /// </summary>
        public void PrintScalarValues(float[] embedding)
        {
            for (int i = 0; i < Math.Min(10, embedding.Length); i++)
            {
                _logger.LogInformation("Value {Index}: {Value}", i + 1, embedding[i]);
            }
        }

        /// <summary>
        /// Calculates cosine similarity between two float vectors.
        /// </summary>
        public double CalculateSimilarity(float[] embedding1, float[] embedding2)
        {
            try
            {
                if (embedding1.Length != embedding2.Length || embedding1.Length == 0)
                    return 0;

                double dotProduct = 0.0;
                double magnitude1 = 0.0;
                double magnitude2 = 0.0;

                for (int i = 0; i < embedding1.Length; i++)
                {
                    dotProduct += embedding1[i] * embedding2[i];
                    magnitude1 += Math.Pow(embedding1[i], 2);
                    magnitude2 += Math.Pow(embedding2[i], 2);
                }

                magnitude1 = Math.Sqrt(magnitude1);
                magnitude2 = Math.Sqrt(magnitude2);

                if (magnitude1 == 0.0 || magnitude2 == 0.0)
                    return 0;

                return dotProduct / (magnitude1 * magnitude2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cosine similarity");
                return 0;
            }
        }

        private async Task<string> ReadTextContentAsync(string filePath)
        {
            if (filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            else if (filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using var document = UglyToad.PdfPig.PdfDocument.Open(filePath);
                    var textBuilder = new System.Text.StringBuilder();

                    foreach (var page in document.GetPages())
                    {
                        textBuilder.AppendLine(page.Text);
                    }

                    return textBuilder.ToString();
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Failed to extract text from PDF: {FilePath}", filePath);
                    return string.Empty;
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// Not implemented. Included for interface completeness.
        /// </summary>
        public Task<double> CalculateSimilarityAsync(string text1, string text2)
        {
            throw new NotImplementedException();
        }

        Task<double> IEmbedding.CalculateEmbeddingAsync(string text1, string text2, string fileName1, string fileName2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads training data for classification by generating embeddings and associated domain labels
        /// from the structured source folders (e.g., Source/Job, Source/Healthcare).
        /// </summary>
        /// <param name="baseSourceFolder">
        /// The root folder path containing subdirectories named by domain (e.g., "Job", "Finance", etc.).
        /// Each subfolder contains text or PDF documents used as training examples for that domain.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// - A list of float arrays representing embeddings for each document.
        /// - A list of corresponding domain labels for those embeddings (same order).
        /// </returns>
        /// <remarks>
        /// This method processes all subfolders under <paramref name="baseSourceFolder"/>.
        /// Each document is read and converted to an embedding using OpenAI.
        /// Embeddings are averaged if documents exceed chunk size and saved to disk
        /// using a suffix (e.g., "_embedding1.txt") to prevent recomputation.
        /// </remarks>
        public async Task<(List<float[]> embeddings, List<string> labels)> LoadTrainingDataAsync(string baseSourceFolder)
        {
            var embeddings = new List<float[]>(); // List to store vector embeddings of all documents
            var labels = new List<string>();      // List to store domain/category label for each embedding

            // Iterate over each subfolder in the base source directory (each represents a domain)
            foreach (var domainFolder in Directory.GetDirectories(baseSourceFolder))
            {
                var domain = Path.GetFileName(domainFolder); // e.g., "Job", "Healthcare", etc.

                // Get all .txt and .pdf files within the current domain folder
                var files = Directory.GetFiles(domainFolder, "*.*")
                                     .Where(f => f.EndsWith(".txt") || f.EndsWith(".pdf"));

                // Process each file in the current domain folder
                foreach (var file in files)
                {
                    // Read text content from the file (supporting both TXT and PDF)
                    string content = await ReadTextContentAsync(file);

                    // Generate (or reuse if already cached) the averaged embedding for the document
                    float[] embedding = await GetAveragedEmbeddingAsync(content, file, Constants.EmbeddingValues1Suffix);

                    // Only add non-empty embeddings to the training set
                    if (embedding.Length > 0)
                    {
                        embeddings.Add(embedding); // Add the embedding vector to the list
                        labels.Add(domain);        // Add the corresponding domain label
                    }
                }
            }

            // Return both embeddings and their labels for training use (e.g., KNN)
            return (embeddings, labels);
        }

    }
}
