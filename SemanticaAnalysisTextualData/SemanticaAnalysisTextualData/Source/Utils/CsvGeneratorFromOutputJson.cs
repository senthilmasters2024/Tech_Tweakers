using CsvGenerator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace CsvGenerator
{
    /// <summary>
    /// Class to Data Model To generate CSV file from Phrase Similarity Score
    /// </summary>
    public class PhrasePair
    {
        /// <summary>
        /// Phrase 1 compare
        /// </summary>
        public string? Phrase1 { get; set; }
        /// <summary>
        /// Phrase 2 compare
        /// </summary>
        public string? Phrase2 { get; set; }
        /// <summary>
        /// Domain we are trying to compare the both the phrases from
        /// </summary>
        public string? Domain { get; set; }
        /// <summary>
        /// Context of the Comparison
        /// </summary>
        public string? Context { get; set; }
        /// <summary>
        /// Output Similarity Score from the Model
        /// </summary>
        public double? SimilarityScore { get; set; }
    }

    /// <summary>
    /// Class To declare List for PhrasePairs
    /// </summary>
    public class JsonData : ISerializable
    {
        /// <summary>
        /// PhrasePairs is a list name
        /// </summary>
        /// 
        [JsonPropertyName("phrase_pairs")]
        public List<PhrasePair> PhrasePairs { get; set; } = new List<PhrasePair>();

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class CsvGeneratorFromOutputJson
    {
        /*static void Main(string[] args)
        {
            string currentDir = Directory.GetCurrentDirectory();
            // Define relative paths
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Gets the directory where the executable is running
            Console.WriteLine($"Base Directory: {baseDir}");
            string relativeJsonPath = Path.Combine(baseDir, "output_dataset.json");
            Console.WriteLine($"File Path: {relativeJsonPath}");
            string relativeCsvPath = Path.Combine(baseDir, "phrase_pairs_dataset.csv");

            try
            {
                // Read JSON and parse
                List<PhrasePair> jsonData = ReadJsonFile(relativeJsonPath);

                if (jsonData != null)
                {
                    // Generate CSV
                    GenerateCsv(jsonData, relativeCsvPath);
                }

                Console.WriteLine($"CSV file generated successfully at: {Path.GetFullPath(relativeCsvPath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
*/
        /// <summary>
        /// Reads the JSON file and parses it into a list of PhrasePair objects.
        /// </summary>
        /// <param name="jsonFilePath">The path to the JSON file.</param>
        /// <returns>A list of PhrasePair objects parsed from the JSON file.</returns>
        public static List<PhrasePair> ReadJsonFile(string jsonFilePath)
        {
            string jsonContent = File.ReadAllText(jsonFilePath);

            var data = System.Text.Json.JsonSerializer.Deserialize<JsonData>(jsonContent);
            if (data == null || data.PhrasePairs == null || data.PhrasePairs.Count == 0)
            {
                throw new InvalidOperationException("The JSON file is either empty or invalid.");
            }

            return data.PhrasePairs;
        }

        static void GenerateCsv(List<PhrasePair> jsonData, string filePath)
        {
            var sb = new StringBuilder();

            // Add headers
            sb.AppendLine("Phrase1,Phrase2,Domain,Context,SimilarityScore");

            // Add data rows
            foreach (var pair in jsonData)
            {
                sb.AppendLine($"{pair.Phrase1},{pair.Phrase2},{pair.Domain},{pair.Context},{pair.SimilarityScore}");
            }

            // Write to CSV file
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
