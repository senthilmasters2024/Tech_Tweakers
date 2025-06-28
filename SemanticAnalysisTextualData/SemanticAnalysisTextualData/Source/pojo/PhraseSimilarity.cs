namespace SemanticAnalysisTextualData.Source.pojo
{
    /// <summary>
    /// Represents the similarity between two phrases.
    /// </summary>
    public class PhraseSimilarity : PhraseSimilarityInputCSV
    {

        /// <summary>
        /// Gets or sets the similarity score between the two phrases.
        /// </summary>
        public double SimilarityScore { get; set; }
    }
}
