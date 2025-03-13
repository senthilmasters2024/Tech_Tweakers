//using CsvGenerator;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace CsvGenerator.Tests
//{
//    [TestClass]
//    public class CsvGeneratorFromOutputJsonTests
//    {
//        [TestMethod]
//        public void ReadJsonFile_ValidJson_ReturnsPhrasePairs()
//        {
//            // Arrange
//            string jsonContent = @"
//            {
//                ""phrase_pairs"": [
//                    {
//                        ""Phrase1"": ""Angela Merkel"",
//                        ""Phrase2"": ""Government"",
//                        ""Domain"": ""Politics"",
//                        ""Context"": ""Leadership"",
//                        ""SimilarityScore"": 0.2192243492408946
//                    },
//                    {
//                        ""Phrase1"": ""Cristiano Ronaldo"",
//                        ""Phrase2"": ""Government"",
//                        ""Domain"": ""Sports"",
//                        ""Context"": ""Irrelevant"",
//                        ""SimilarityScore"": 0.10349840488983812
//                    }
//                ]
//            }";
//            string jsonFilePath = Path.GetTempFileName();
//            File.WriteAllText(jsonFilePath, jsonContent);

//            // Act
//            List<PhrasePair> result = CsvGeneratorFromOutputJson.ReadJsonFile(jsonFilePath);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Angela Merkel", result[0].Phrase1);
//            Assert.AreEqual("Government", result[0].Phrase2);
//            Assert.AreEqual("Politics", result[0].Domain);
//            Assert.AreEqual("Leadership", result[0].Context);
//            Assert.AreEqual(0.2192243492408946, result[0].SimilarityScore);
//            Assert.AreEqual("Cristiano Ronaldo", result[1].Phrase1);
//            Assert.AreEqual("Government", result[1].Phrase2);
//            Assert.AreEqual("Sports", result[1].Domain);
//            Assert.AreEqual("Irrelevant", result[1].Context);
//            Assert.AreEqual(0.10349840488983812, result[1].SimilarityScore);

//            // Clean up
//            File.Delete(jsonFilePath);
//        }

//        [TestMethod]
//        public void ReadJsonFile_InvalidJson_ThrowsInvalidOperationException()
//        {
//            // Arrange
//            string jsonContent = @"{ ""invalid_json"": [] }";
//            string jsonFilePath = Path.GetTempFileName();
//            File.WriteAllText(jsonFilePath, jsonContent);

//            // Act & Assert
//            Assert.ThrowsException<InvalidOperationException>(() => CsvGeneratorFromOutputJson.ReadJsonFile(jsonFilePath));

//            // Clean up
//            File.Delete(jsonFilePath);
//        }
//    }
//}
