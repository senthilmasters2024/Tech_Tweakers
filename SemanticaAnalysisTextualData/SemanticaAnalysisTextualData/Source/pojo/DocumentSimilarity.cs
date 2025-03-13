using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.pojo
{
    /// <summary>
    /// Represents the similarity between two documents.
    /// </summary>
    public class DocumentSimilarity
    {
        internal string Domain;

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
