using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.Interfaces
{
    /// <summary>
    /// Interface for embedding and calculating similarity between textual data.
    /// </summary>
    internal interface IEmbedding
    {
        /// <summary>
        /// Calculates the similarity between two texts asynchronously.
        /// </summary>
        /// <param name="text1">The first text to compare.</param>
        /// <param name="text2">The second text to compare.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the similarity score between the two texts.</returns>
        Task<double> CalculateSimilarityAsync(string text1, string text2);
    }
}