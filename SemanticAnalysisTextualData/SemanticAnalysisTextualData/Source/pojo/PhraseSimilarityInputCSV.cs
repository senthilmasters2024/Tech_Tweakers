namespace SemanticAnalysisTextualData.Source.pojo
{
    /// <summary>
    /// Represents the similarity between two phrases.
    /// </summary>
    public class PhraseSimilarityInputCSV
    {
        /// <summary>
        /// Gets or sets the first phrase.
        /// </summary>
        public string? Phrase1 { get; set; }

        /// <summary>
        /// Gets or sets the second phrase.
        /// </summary>
        public string? Phrase2 { get; set; }

        /// <summary>
        /// Gets or sets the domain of the phrases.
        /// </summary>
        public string? Domain { get; set; }

        /// <summary>
        /// Gets or sets the context of the phrases.
        /// </summary>
        public string? Context { get; set; }
    }

}
