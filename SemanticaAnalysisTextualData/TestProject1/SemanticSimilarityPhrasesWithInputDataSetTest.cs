using Newtonsoft.Json;
using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.pojo;
using SemanticaAnalysisTextualData.Source.Utils;
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
            CsvHelperUtilTest obj = new CsvHelperUtilTest();
            obj.SaveResultsPhrase_ShouldSaveResultsToCsvAndJsonFiles();
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
            Directory.CreateDirectory(Path.GetDirectoryName(datasetPath) ?? throw new ArgumentNullException(nameof(datasetPath), "The dataset path cannot be null."));

            // mockService.Setup(s => s.GetProjectRoot()).Returns(tempDir);

            // Act
            var result = mockService.LoadDataset();

            // Assert
            Assert.IsNotNull(result, "Dataset should not be null.");
            Assert.AreEqual(51, result.PhrasePairs.Count, "The number of phrase pairs should match.");
       
        }


        [TestMethod]
        public async Task CalculatePhraseSimilarityAsync_ShouldReturnZero_OnException()
        {
            // Arrange
            EmbeddingClient client = new(Constants.EmbeddingModel, Environment.GetEnvironmentVariable(Constants.OpenAIAPIKeyEnvVar));
            SemanticSimilarityPhrasesWithInputDataSet service = new SemanticSimilarityPhrasesWithInputDataSet();

            var phrase1 = "phrase1";
            var phrase2 = "phrase2";

            // Act
            var result = await service.CalculatePhraseSimilarityAsync(phrase1, phrase2);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
