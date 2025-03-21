using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.Utils
{
    /// <summary>
    /// Contains constant values used throughout the application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The base data folder path.
        /// </summary>
        public const string BaseDataFolder = "data";

        /// <summary>
        /// The folder path for preprocessed source files based on domains.
        /// </summary>
        public const string ProcessedSourceFolder = "PreprocessedSourceBasedOnDomains";

        /// <summary>
        /// The folder path for preprocessed source files based on needed relevance.
        /// </summary>
        public const string ProcessedTargetFolder = "PreprocessedSourceBasedOnNeededRelevance";

       /// <summary>
       /// The folder path for  source files based on needed relevance.
       /// </summary>
        public const string SourceFolder = "SourceBasedOnDomains";

        /// <summary>
        /// The folder path for  target files based on needed relevance.
        /// </summary>
        public const string TargetFolder = "SourceBasedOnNeededRelevance";

        /// <summary>
        /// The embedding model name.
        /// </summary>
        public const string EmbeddingModel = "text-embedding-3-large";

        /// <summary>
        /// The environment variable name for the OpenAI API key.
        /// </summary>
        public const string OpenAIAPIKeyEnvVar = "OPENAI_API_KEY";

        /// <summary>
        /// The domain name for job profiles.
        /// </summary>
        public const string JobProfileDomain = "jobvacancy";

        /// <summary>
        /// The domain name for medical history.
        /// </summary>
        public const string MedicalHistoryDomain = "Medical-MedicationSuggestion";

        /// <summary>
        /// The domain name for unknown domains.
        /// </summary>
        public const string UnknownDomain = "Unknown";

        /// <summary>
        /// The file extension for text files.
        /// </summary>
        public const string TextFileExtension = "*.txt";

        /// <summary>
        /// The suffix for embedding values CSV files.
        /// </summary>
        public const string EmbeddingValuesSuffix = "embedding_values.csv";

        /// <summary>
        /// The suffix for the second set of embedding values CSV files.
        /// </summary>
        public const string EmbeddingValues1Suffix = "embedding_values1.csv";
    }
}
