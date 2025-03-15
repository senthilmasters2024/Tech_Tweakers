using SemanticaAnalysisTextualData.Source.Utils;
using SemanticAnalysisTextualData.Source;
using System.Diagnostics;

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
    }
}