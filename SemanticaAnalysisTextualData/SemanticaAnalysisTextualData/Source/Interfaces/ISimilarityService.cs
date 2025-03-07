namespace SemanticaAnalysisTextualData.Source.Interfaces
{
    /// <summary>
    /// Interface for calculating similarity between textual data.
    /// </summary>
    public interface ISimilarityService
    {
        /// <summary>
        /// Calculates the similarity between two texts asynchronously.
        /// </summary>
        /// <param name="text1">The first text.</param>
        /// <param name="text2">The second text.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score.</returns>
        Task<double> CalculateSimilarityAsync(string text1, string text2);

        /// <summary>
        /// Calculates the similarity between two embeddings.
        /// </summary>
        /// <param name="embedding1">The first embedding.</param>
        /// <param name="embedding2">The second embedding.</param>
        /// <returns>The similarity score.</returns>
        double CalculateSimilarity(float[] embedding1, float[] embedding2);
    }
}
