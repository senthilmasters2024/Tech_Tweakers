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
        [TestMethod]
        public void PreprocessText_ShouldNormalizeContractions()
        {
            string input = "I can't do this";
            string expected = "i cannot do this";
            string result = _preprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }
    }
}
