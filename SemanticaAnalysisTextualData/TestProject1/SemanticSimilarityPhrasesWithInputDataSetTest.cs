using Newtonsoft.Json;
using SemanticaAnalysisTextualData.Source.pojo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SemanticaAnalysisTextualData.Util.CsvHelperUtil;

namespace SemanticaAnalysisTextualDataTest
{
    [TestClass]
    public class SemanticSimilarityPhrasesWithInputDataSetTest
    {
        [TestMethod]
        public async Task InvokeProcessPhrases_ShouldProcessPhrasesAndSaveResults()
        {
            // Arrange
            SemanticSimilarityPhrasesWithInputDataSet mockService = new SemanticSimilarityPhrasesWithInputDataSet();
            var mockResults = new List<PhraseSimilarity>
            {
                new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 },
                new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 }
            };
            String[] S = new string[0];
            await  mockService.InvokeProcessPhrases(S);

            // Act


            // Assert
            Assert.Inconclusive("Method Does Not Return Anything but no Errors are thrown.");

        }

        [TestMethod]
        public void LoadDataset_ShouldReturnDataset_WhenFileExists()
        {
            // Arrange
            // Arrange
            SemanticSimilarityPhrasesWithInputDataSet mockService = new SemanticSimilarityPhrasesWithInputDataSet();
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            string datasetPath = Path.Combine(tempDir, "data", "InputPhrases50DataSet.json");
            Directory.CreateDirectory(Path.GetDirectoryName(datasetPath));

            // mockService.Setup(s => s.GetProjectRoot()).Returns(tempDir);

            // Act
            var result = mockService.LoadDataset();

            // Assert
            Assert.IsNotNull(result, "Dataset should not be null.");
            Assert.AreEqual(51, result.PhrasePairs.Count, "The number of phrase pairs should match.");
       
        }
    }
}
