using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalysisTextualData.Source.pojo
{
    /// <summary>
    /// Represents the input dataset containing a list of phrase pairs.
    /// </summary>
    public class InputDataset
    {
        /// <summary>
        /// Gets or sets the list of phrase pairs.
        /// </summary>
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new();
    }
}
