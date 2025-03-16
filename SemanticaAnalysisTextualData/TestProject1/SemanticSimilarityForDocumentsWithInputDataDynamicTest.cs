using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.Utils;
using SemanticAnalysisTextualData.Source;
using System.Diagnostics;
using SemanticaAnalysisTextualData.Source.pojo;
using Microsoft.Extensions.DependencyInjection;

namespace SemanticaAnalysisTextualDataTest
{
    [TestClass]
    public class SemanticSimilarityForDocumentsWithInputDataDynamicTest
    {

       
        [TestMethod]
        public void SampleTestMethod()
        {
            SemanticSimilarityForDocumentsWithInputDataDynamic obj = new SemanticSimilarityForDocumentsWithInputDataDynamic();

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public async Task CompareDocumentsAsync_ShouldHandleExceptions()
        {
            // Arrange
            SemanticSimilarityForDocumentsWithInputDataDynamic service = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            // Define the base data folder and subfolders for source and target files
            // Get the project root directory
            // Get the project directory for which the test is being created
            string? projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectRoot == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }

            string baseDataFolder = Path.Combine(projectRoot, Constants.BaseDataFolder);
            string sourceFolder = Path.Combine(baseDataFolder, Constants.SourceFolder);
            string targetFolder = Path.Combine(baseDataFolder, Constants.TargetFolder);
            // Get the list of source and target files
            var sourceFiles = Directory.GetFiles(sourceFolder, Constants.TextFileExtension);
            var targetFiles = Directory.GetFiles(targetFolder, Constants.TextFileExtension);

            // Act
            var results = await service.CompareDocumentsAsync(sourceFiles, targetFiles);

            // Assert
            Assert.IsNotNull(results);
            //Assert.AreEqual(0, results.Count); // Expecting an empty list due to file not found exceptions
        }

        [TestMethod]
        public async Task CalculateEmbeddingAsync_ShouldReturnSimilarityScore()
        {
            SemanticSimilarityForDocumentsWithInputDataDynamic service = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            // Arrange
            string? projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectRoot == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }

            string baseDataFolder = Path.Combine(projectRoot, Constants.BaseDataFolder);
            string sourceFolder = Path.Combine(baseDataFolder, Constants.SourceFolder);
            string targetFolder = Path.Combine(baseDataFolder, Constants.TargetFolder);
            // Get the list of source and target files
            var sourceFiles = Directory.GetFiles(sourceFolder, Constants.TextFileExtension);
            var targetFiles = Directory.GetFiles(targetFolder, Constants.TextFileExtension);

            EmbeddingClient client = new(Constants.EmbeddingModel, Environment.GetEnvironmentVariable(Constants.OpenAIAPIKeyEnvVar));

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
                    var similarity = await service.CalculateEmbeddingAsync(sentence1, sentence2, fileName1, fileName2);
                    Console.WriteLine($"Similarity between {fileName1} and {fileName2}: {similarity:F4}");

                    // Create a DocumentSimilarity object and add it to the results list
                    var phraseSimilarity = service.CreateDocumentSimilarity(fileName1, fileName2, similarity);
                    Console.WriteLine($"File: {sourceFile}, Domain: {phraseSimilarity.domain}");
                    results.Add(phraseSimilarity);
                }
            }

            // Assert
            foreach (var result in results)
            {
                Assert.IsTrue(result.SimilarityScore > 0, $"Similarity score for {result.FileName1} and {result.FileName2} should be greater than 0.");
            }
        }

        [TestMethod]
        public void ConfigureServices_ShouldReturnServiceProviderWithConfiguredServices()
        {
            // Act
            var serviceProvider = SemanticSimilarityForDocumentsWithInputDataDynamic.ConfigureServices();

            // Assert
            Assert.IsNotNull(serviceProvider);
            var service = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(SemanticSimilarityForDocumentsWithInputDataDynamic));
        }

        [TestMethod]
        public void GetSourceAndTargetFiles_ShouldReturnSourceAndTargetFiles()
        {
            // Arrange
            var service = new SemanticSimilarityForDocumentsWithInputDataDynamic();

            // Act
            var (sourceFiles, targetFiles) = service.GetSourceAndTargetFiles();

            // Assert
            Assert.IsNotNull(sourceFiles);
            Assert.IsNotNull(targetFiles);
            Assert.IsTrue(sourceFiles.Length > 0, "Source files should not be empty.");
            Assert.IsTrue(targetFiles.Length > 0, "Target files should not be empty.");
            Assert.IsTrue(sourceFiles.All(file => file.EndsWith(".txt")), "All source files should have the correct extension.");
            Assert.IsTrue(targetFiles.All(file => file.EndsWith(".txt")), "All target files should have the correct extension.");
        }

        [TestMethod]
        public void PrintScalarValues_ShouldPrintEachScalarValue()
        {
            // Arrange
            float[] embedding = { 1.0f, 2.0f, 3.0f };
            var expectedOutput = "Word 1: 1\r\nWord 2: 2\r\nWord 3: 3\r\n";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                SemanticSimilarityForDocumentsWithInputDataDynamic.PrintScalarValues(embedding);

                // Assert
                var result = sw.ToString();
                Assert.AreEqual(expectedOutput, result);
            }
        }

        [TestMethod]
        public void CalculateSimilarity_ShouldReturnCorrectSimilarityScore()
        {
            // Arrange
            var service = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            float[] embedding1 = { 1.0f, 2.0f, 3.0f };
            float[] embedding2 = { 1.0f, 2.0f, 3.0f };
            float[] embedding3 = { 4.0f, 5.0f, 6.0f };
            float[] embedding4 = { 0.0f, 0.0f, 0.0f };

            // Act
            var similarity1 = service.CalculateSimilarity(embedding1, embedding2);
            var similarity2 = service.CalculateSimilarity(embedding1, embedding3);
            var similarity3 = service.CalculateSimilarity(embedding1, embedding4);

            // Assert
            Assert.AreEqual(1.0, similarity1, 0.0001, "Similarity between identical embeddings should be 1.");
            Assert.IsTrue(similarity2 > 0 && similarity2 < 1, "Similarity between different embeddings should be between 0 and 1.");
            Assert.AreEqual(0, similarity3, "Similarity with zero magnitude embedding should be 0.");
        }

        [TestMethod]
        public void CalculateSimilarity_ShouldReturnZeroForDifferentLengthEmbeddings()
        {
            // Arrange
            var service = new SemanticSimilarityForDocumentsWithInputDataDynamic();
            float[] embedding1 = { 1.0f, 2.0f, 3.0f };
            float[] embedding5 = { 1.0f, 2.0f };

            // Act
            var similarity = service.CalculateSimilarity(embedding1, embedding5);

            // Assert
            Assert.AreEqual(0, similarity, "Similarity between embeddings of different lengths should be 0.");
        }
    }
}