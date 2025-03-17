using CsvHelper;
using Newtonsoft.Json;
using SemanticaAnalysisTextualData.Source.pojo;
using SemanticaAnalysisTextualData.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SemanticaAnalysisTextualData.Util.CsvHelperUtil;

namespace SemanticaAnalysisTextualDataTest
{

    [TestClass]
    public class CsvHelperUtilTest
    {


        [TestMethod]
        public void SaveResultsToCsv_ShouldSaveResultsToCsvFile()
        {
            // Arrange
            var results = new List<DocumentSimilarity>
            {
                new DocumentSimilarity { FileName1 = "file1.txt", FileName2 = "file2.txt", domain = "domain1", SimilarityScore = 0.9 },
                new DocumentSimilarity { FileName1 = "file3.txt", FileName2 = "file4.txt", domain = "domain2", SimilarityScore = 0.8 }
            };

            string currentDir = Directory.GetCurrentDirectory();
            string outputPath = Path.Combine(currentDir, "data", "output_dataset.csv");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? throw new ArgumentNullException(nameof(outputPath)));

            // Act
            CsvHelperUtil.SaveResultsToCsv(results);

            // Assert
            Assert.IsTrue(File.Exists(outputPath), "CSV file should be created.");

            using (var reader = new StreamReader(outputPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var readResults = csv.GetRecords<DocumentSimilarity>().ToList();
                Assert.AreEqual(results.Count, readResults.Count, "The number of records should match.");
                for (int i = 0; i < results.Count; i++)
                {
                    Assert.AreEqual(results[i].FileName1, readResults[i].FileName1);
                    Assert.AreEqual(results[i].FileName2, readResults[i].FileName2);
                    Assert.AreEqual(results[i].domain, readResults[i].domain);
                    Assert.AreEqual(results[i].SimilarityScore, readResults[i].SimilarityScore);
                }
            }

            // Clean up
            //Directory.Delete(tempDir, true);
        }

        [TestMethod]
        public void SaveResultsPhrase_ShouldSaveResultsToCsvAndJsonFiles()
        {
            // Arrange
            var results = new List<PhraseSimilarity>
            {
                new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 },
                new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 }
            };

            // Get the current directory and define the output path for the CSV file
            string currentDir = Directory.GetCurrentDirectory();
            string outputPath = Path.Combine(currentDir, "data", "output_dataset.csv");
            string outputPathJson = Path.Combine(currentDir, "data", "output_dataset.json");
            string outputPathCsv = Path.Combine(currentDir, "data", "output_datasetphrases.csv");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPathJson) ?? throw new ArgumentNullException(nameof(outputPathJson)));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPathCsv) ?? throw new ArgumentNullException(nameof(outputPathCsv)));

            // Act
            CsvHelperUtil.SaveResultsPhrase(results);

            // Assert JSON file
            Assert.IsTrue(File.Exists(outputPathJson), "JSON file should be created.");
            var jsonContent = File.ReadAllText(outputPathJson);
            var deserializedObj = JsonConvert.DeserializeObject<InputDataset>(jsonContent);
            Assert.IsNotNull(deserializedObj);
            Assert.AreEqual(results.Count, deserializedObj.PhrasePairs.Count, "The number of records in JSON should match.");

            // Assert CSV file
            Assert.IsTrue(File.Exists(outputPathCsv), "CSV file should be created.");
            using (var reader = new StreamReader(outputPathCsv))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var readResults = csv.GetRecords<PhraseSimilarity>().ToList();
                Assert.AreEqual(results.Count, readResults.Count, "The number of records in CSV should match.");
                for (int i = 0; i < results.Count; i++)
                {
                    Assert.AreEqual(results[i].Phrase1, readResults[i].Phrase1);
                    Assert.AreEqual(results[i].Phrase2, readResults[i].Phrase2);
                    Assert.AreEqual(results[i].Domain, readResults[i].Domain);
                    Assert.AreEqual(results[i].Context, readResults[i].Context);
                    Assert.AreEqual(results[i].SimilarityScore, readResults[i].SimilarityScore);
                }
            }

            // Clean up
           // Directory.Delete(currentDir, true);
        }
    }
}
