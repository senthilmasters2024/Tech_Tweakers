using CsvHelper;
using Newtonsoft.Json;
using SemanticAnalysisTextualData.Source.pojo;
using System.Globalization;

namespace SemanticAnalysisTextualData.Util
{
    /// <summary>
    /// Provides utility methods for working with CSV files.
    /// </summary>
    public static class CsvHelperUtil
    {
        /// <summary>
        /// Saves the results to a CSV file.
        /// </summary>
        /// <param name="results">The list of DocumentSimilarity results to save.</param>
        public static void SaveResultsToCsv(List<DocumentSimilarity> results)
        {
            // Get the current directory and define the output path for the CSV file
            string currentDir = Directory.GetCurrentDirectory();
            string outputPath = Path.Combine(currentDir, "data", "output_dataset.csv");

            // Write the results to the CSV file
            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(results);
            }
            Console.WriteLine($"Results saved to {outputPath}.");
        }

        /// <summary>
        /// Saves the results to a CSV file.
        /// </summary>
        /// <param name="results">The list of PhraseSimilarity results to save.</param>
        public static void SaveResultsPhrase(List<PhraseSimilarity> results)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string outputPathJson = Path.Combine(currentDir, "data", "output_dataset.json");
            string outputPathCsv = Path.Combine(currentDir, "data", "output_datasetphrases.csv");

            var obj = new InputDataset { PhrasePairs = results };

            File.WriteAllText(outputPathJson, JsonConvert.SerializeObject(obj, Formatting.Indented));
            Console.WriteLine("Results saved to output_dataset.json.");

            using (var writer = new StreamWriter(outputPathCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(results);
            }
            Console.WriteLine($"Phrase Similarity Results saved to {outputPathCsv}.");
        }

        /// <summary>
        /// Represents a dataset containing phrase pairs.
        /// </summary>
        public class InputDataset
        {
            /// <summary>
            /// Gets or sets the list of phrase pairs.
            /// </summary>
            [JsonProperty("phrase_pairs")]
            public List<PhraseSimilarity> PhrasePairs { get; set; } = new List<PhraseSimilarity>();
        }
    }
}
