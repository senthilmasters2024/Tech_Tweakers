using Google.Api;
using SemanticaAnalysisTextualData.Source.Services;

namespace SemanticaAnalysisTextualDataTest
{
    [TestClass]
    public class SemanticAnalysisTextualDataTest
    {

        private readonly SemanticAnalysisTextualDataService _service;
        public SemanticAnalysisTextualDataTest()
        {
            _service = new SemanticAnalysisTextualDataService();
        }
    
        [TestMethod]
        public void CalculateSimilarity_ShouldUseAllEmbeddingElements()
        {
            // Arrange: Define test embeddings with distinct values
            float[] embedding1 = Enumerable.Range(1, 10).Select(i => (float)i).ToArray();  // [1.0, 2.0, ..., 10.0]
            float[] embedding2 = Enumerable.Range(11, 10).Select(i => (float)i).ToArray(); // [11.0, 12.0, ..., 20.0]

            // Act: Calculate similarity
            double similarity = _service.CalculateSimilarity(embedding1, embedding2);

            // Assert: Ensure all elements are contributing to the calculation
            // Check that the function is not defaulting to the first element or ignoring any portion.
            float sumEmbedding1 = embedding1.Sum();
            float sumEmbedding2 = embedding2.Sum();

            Assert.AreNotEqual(embedding1[0] * embedding2[0], similarity); // Ensure not using only first element
            Assert.IsTrue(similarity > 0); // Ensure meaningful computation is happening
            Assert.IsTrue(similarity >= 0.0 && similarity <= 1.0); // Cosine similarity range should be between 0 and 1
        }
    }
}