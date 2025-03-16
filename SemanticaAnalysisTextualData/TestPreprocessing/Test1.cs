using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestPreprocessing
{
    [TestClass]
    public class TextPreprocessorTests
    {
        private TextPreprocessor _preprocessor;

        [TestInitialize]
        public void Setup()
        {
            _preprocessor = new TextPreprocessor();
        }
        [TestMethod]
        public void PreprocessText_ShouldConvertToLowercase()
        {
            string input = "THE Goal of AI is to provide SOFTWARE that can reason on Input and explain on Output. ";
            string expected = "the goal of AI is to provide software that can reason on input and explain on output.";
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Console.WriteLine($"Actual Output: {result}");
            Assert.AreEqual(expected, result);
        }
             public void PreprocessText_ShouldRemoveHtmlTagsAndUrls()
        {
            string input = "<b>Hello</b> Visit http://example.com";
            string expected = "hello visit";  // URL and tags should be removed
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void PreprocessText_ShouldNormalizeContractions()
        {
            string input = "I can't do this";
            string expected = "i cannot do this";
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void PreprocessText_ShouldRemoveSpecialCharacters()
        {
            string input = "Hello, world!";
            string expected = "hello world";
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PreprocessText_ShouldApplyStopWordFilteringForWord()
        {
            string input = "the";
            string expected = ""; // Stop words should be removed
            string result = _preprocessor.PreprocessText(input, TextDataType.Word);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PreprocessText_ShouldHandlePhrases()
        {
            string input = "The quick brown fox jumps";
            string expected = "quick brown fox jumps"; // "The" is a stop word
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PreprocessText_ShouldHandleDocuments()
        {
            string input = "Hello! This is a test. I am running.";
            string expected = "hello. this test. i run";  // Stop words removed and lemmatization applied
            string result = _preprocessor.PreprocessText(input, TextDataType.Document);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LemmatizeWord_ShouldReturnLemmatizedWord()
        {
            string input = "running";
            string expected = "run";
            string result = TextPreprocessor.LemmatizeWord(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LemmatizeText_ShouldApplyLemmatizationToSentence()
        {
            string input = "running better went";
            string expected = "run good go";
            string result = TextPreprocessor.LemmatizeText(input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task SaveWordsAsync_ShouldCreateFile()
        {
            string domainName = "TestDomain";
            List<string> words = new() { "word1", "word2", "word3" };
            string outputFolder = "TestOutput";

            await _preprocessor.SaveWordsAsync(domainName, words, outputFolder);

            string filePath = System.IO.Path.Combine(outputFolder, "Words", "preprocessed_words.txt");
            Assert.IsTrue(System.IO.File.Exists(filePath));

            var savedWords = await System.IO.File.ReadAllLinesAsync(filePath);
            CollectionAssert.AreEqual(words, savedWords);
        }

        [TestMethod]
        public async Task LoadPreprocessedWordsAsync_ShouldReturnStoredWords()
        {
            string domainName = "TestDomain";
            string outputFolder = "TestOutput";
            string filePath = System.IO.Path.Combine(outputFolder, "Words", domainName, "preprocessed_words.txt");

            // Ensure the file exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            await System.IO.File.WriteAllLinesAsync(filePath, new[] { "word1", "word2" });

            var result = await _preprocessor.LoadPreprocessedWordsAsync(domainName, outputFolder);
            CollectionAssert.AreEqual(new List<string> { "word1", "word2" }, new List<string>(result));
        }
    }

}

