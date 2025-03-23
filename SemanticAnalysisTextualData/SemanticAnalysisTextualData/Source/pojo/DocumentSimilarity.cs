namespace SemanticAnalysisTextualData.Source.pojo
{
    /// <summary>
    /// Represents the similarity between two documents.
    /// </summary>
    public class DocumentSimilarity
    {

        /// <summary>
        /// Gets or sets the name of the first file.
        /// </summary>
        public string? FileName1 { get; set; }

        /// <summary>
        /// Gets or sets the name of the second file.
        /// </summary>
        public string? FileName2 { get; set; }

        /// <summary>
        /// Gets or sets the domain of the documents.
        /// </summary>
        public string? domain { get; set; }

        /// <summary>
        /// Gets or sets the similarity score between the two documents.
        /// </summary>
        public double SimilarityScore { get; set; }
    }
}
