namespace SemanticAnalysisTextualData
{
    /// <summary>
    /// Unit tests for the TextPreprocessor class to ensure correct functionality of its methods.
    /// </summary>
    [TestClass]
    public class TextPreprocessorTests
    {
        TextPreprocessor _textPreprocessor = new TextPreprocessor();

        /// <summary>
        /// Initializes the test setup.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _textPreprocessor = new TextPreprocessor();
        }

        /// <summary>
        /// Tests the InitializeTextData method to ensure it sets properties correctly.
        /// </summary>
        [TestMethod]
        public void InitializeTextData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            string name = "Sample";
            string content = "This is a test.";
            string filePath = "sample.txt";

            // Act
            _textPreprocessor.InitializeTextData(name, content, filePath);

            // Assert
            Assert.AreEqual(name, _textPreprocessor.Name);
            Assert.AreEqual(content, _textPreprocessor.Content);
            Assert.AreEqual(filePath, _textPreprocessor.FilePath);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it converts text to lowercase.
        /// </summary>
        [TestMethod]
        public void PreprocessText_ShouldConvertToLowercase()
        {

            string input = "ARTIFICIAL INTELLIGENCE";
            string expected = "artificial intelligence";
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);
            Console.WriteLine($"Actual Output: {result}");
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it removes HTML tags and URLs.
        /// </summary>
        public void PreprocessText_ShouldRemoveHtmlTagsAndUrls()
        {
            // URL and tags should be removed
            string input = "<b>Hello</b> Visit http://example.com";
            string expected = "hello visit";
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it normalizes contractions.
        /// </summary>
        [TestMethod]
        public void PreprocessText_ShouldNormalizeContractions()
        {
            //Normalization of words
            //Arrange
            string input = "I can't do this";
            string expected = "i cannot do";
            //Act
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it removes special characters.
        /// </summary>
        [TestMethod]
        public void PreprocessText_ShouldRemoveSpecialCharacters()
        {
            //Removed Unwanted words
            //Arrange
            string input = "Work hard, for a better future!";
            string expected = "work hard good future";
            //Act
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it removes stop words.
        /// </summary>
        [TestMethod]
        public void PreprocessText_ShouldRemoveStopWords()
        {
            //Arrange
            string input = "The quick brown fox";
            string expected = "quick brown fox"; // Stop words should be removed
            //Act
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);
            //Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the PreprocessText method to ensure it lemmatizes words.
        /// </summary>
        [TestMethod]
        public void PreprocessText_ShouldLemmatizeWords()
        {
            // Arrange
            string input = "running went better";
            string expected = "run go good";

            // Act
            string result = _textPreprocessor.PreprocessText(input, TextDataType.Phrase);

            // Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the LemmatizeWord method to ensure it returns the correct lemma.
        /// </summary>
        [TestMethod]
        public void LemmatizeWord_ShouldReturnCorrectLemma()
        {
            // No lemma mapping, should return itself
            // Act & Assert
            Assert.AreEqual("run", TextPreprocessor.LemmatizeWord("running"));
            Assert.AreEqual("good", TextPreprocessor.LemmatizeWord("better"));
            Assert.AreEqual("go", TextPreprocessor.LemmatizeWord("went"));
            Assert.AreEqual("be", TextPreprocessor.LemmatizeWord("is"));
            Assert.AreEqual("test", TextPreprocessor.LemmatizeWord("test"));
        }

        /// <summary>
        /// Tests the LemmatizeText method to ensure it lemmatizes a sentence.
        /// </summary>
        [TestMethod]
        public void LemmatizeText_ShouldLemmatizeSentence()
        {
            //Arrange
            string input = "running went better";
            string expected = "run go good";
            //Act
            string result = TextPreprocessor.LemmatizeText(input);
            //Assert

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Tests the SavePhrasesAsync method to ensure it creates a file.
        /// </summary>
        [TestMethod]
        public async Task SavePhrasesAsync_ShouldCreateFile()
        {
            //Save to Output folder
            // Arrange
            string domain = "TestDomain";
            string outputFolder = "TestOutput";
            Directory.CreateDirectory(outputFolder);
            var phrases = new List<string> { "phrase1", "phrase2" };

            // Act
            await _textPreprocessor.SavePhrasesAsync(domain, phrases, outputFolder);

            // Assert
            string filePath = Path.Combine(outputFolder, "Phrases", domain, "preprocessed_phrases.txt");
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            Directory.Delete(outputFolder, true);
        }


        /// <summary>
        /// Tests the SaveDocumentsAsync method to ensure it creates a file.
        /// </summary>
        [TestMethod]
        public async Task SaveDocumentsAsync_ShouldCreateFile()
        {
            //Save to  Output folder
            // Arrange
            string documentType = "TestDocs";
            string outputFolder = "TestOutput";
            Directory.CreateDirectory(outputFolder);
            var documents = new List<string> { "doc1", "doc2" };

            // Act
            await _textPreprocessor.SaveDocumentsAsync(documentType, documents, outputFolder);

            // Assert
            string filePath = Path.Combine(outputFolder, "Documents", "preprocessed_documents.txt");
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            Directory.Delete(outputFolder, true);
        }

        /// <summary>
        /// Tests the SaveWordsAsync method to ensure it creates a file.
        /// </summary>
        [TestMethod]
        public async Task SaveWordsAsync_ShouldCreateFile()
        {
            //Save words to output folder 
            //Arrange
            string domainName = "TestDomain";
            string outputFolder = "TestOutput";
            Directory.CreateDirectory(outputFolder);
            var words = new List<string> { "word1", "word2", "word3" };

            //Act
            await _textPreprocessor.SaveWordsAsync(domainName, words, outputFolder);

            //Assert  
            string filePath = Path.Combine(outputFolder, "Words", "preprocessed_words.txt");
            Assert.IsTrue(File.Exists(filePath));


        }



        /// <summary>
        /// Tests the SaveProcessedContent method to ensure it saves content to a file.
        /// </summary>
        [TestMethod]
        public void SaveProcessedContent_ShouldSaveContentToFile()
        {
            //Processed word saving
            // Arrange
            var preprocessor = new TextPreprocessor();
            string testName = "TestFile";
            string testContent = "This is a test content.";
            string outputFolder = Path.Combine(Path.GetTempPath(), "TextPreprocessorTests");
            Directory.CreateDirectory(outputFolder);
            preprocessor.InitializeTextData(testName, testContent);

            // Act
            preprocessor.SaveProcessedContent(outputFolder);

            // Assert
            string expectedFilePath = Path.Combine(outputFolder, $"{testName}.txt");
            Assert.IsTrue(File.Exists(expectedFilePath), "The file was not created.");
            string savedContent = File.ReadAllText(expectedFilePath);
            Assert.AreEqual(testContent, savedContent, "The content of the file is not as expected.");


        }
    }
}

