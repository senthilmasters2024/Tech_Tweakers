using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SemanticaAnalysisTextualData.Source.Enums;
using Porter2Stemmer;


namespace SemanticaAnalysisTextualData.Source.Services
{
    public class TextPreprocessor : ITextPreprocessor
    {
        private static readonly HashSet<string> StopWords = new()
    {
        "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and"
    };
        private readonly EnglishPorter2Stemmer _stemmer = new EnglishPorter2Stemmer(); // Stemming Library

        public string PreprocessText(string text, TextDataType dataType)
        {
            text = text.ToLower().Trim();
            text = Regex.Replace(text, @"[^a-zA-Z0-9\s]", ""); // Remove special characters

            if (dataType == TextDataType.Word)
            {
                return StopWords.Contains(text) ? string.Empty : text; // Remove stopwords for words
            }
            else if (dataType == TextDataType.Phrase)
            {
                var words = text.Split(' ').Select(word => PreprocessText(word, TextDataType.Word));
                return string.Join(" ", words.Where(w => !string.IsNullOrEmpty(w)));
            }
            else if (dataType == TextDataType.Document)
            {
                var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                var processedSentences = sentences.Select(sentence => PreprocessText(sentence, TextDataType.Phrase));
                return string.Join(". ", processedSentences);
            }

            return text;
        }

        public List<IDocument> LoadDocuments(string folderPath)
        {
            var documents = new List<IDocument>();
            foreach (var filePath in Directory.GetFiles(folderPath, "*.txt"))
            {
                documents.Add(new Document(filePath));
            }
            return documents;
        }

        public async Task ProcessAndSaveDocuments(string inputFolder, string outputFolder)
        {
            var documents = LoadDocuments(inputFolder);

            foreach (var document in documents)
            {
                document.LoadContent();
                string preprocessedContent = PreprocessText(document.Content, TextDataType.Document);

                // Apply stemming after preprocessing
                string stemmedContent = StemText(preprocessedContent);

                // Save the final output
                string outputFilePath = Path.Combine(outputFolder, Path.GetFileName(document.FilePath));
                await File.WriteAllTextAsync(outputFilePath, stemmedContent);
            }
        }

        /// <summary>
        /// Stems each word in the input text using Porter2 stemming.
        /// </summary>
        public string StemText(string text)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var stemmedWords = words.Select(word => _stemmer.Stem(word).Value);
            return string.Join(" ", stemmedWords);
        }

        /// <summary>
        /// Asynchronously applies stemming to all text files in a folder and saves the output.
        /// </summary>
        public async Task StemDocumentsInFolderAsync(string inputFolder, string outputFolder)
        {
            var documents = LoadDocuments(inputFolder);

            List<Task> tasks = new List<Task>();

            foreach (var document in documents)
            {
                tasks.Add(Task.Run(() =>
                {
                    document.LoadContent();
                    string stemmedContent = StemText(document.Content);

                    File.WriteAllText(Path.Combine(outputFolder, Path.GetFileName(document.FilePath)), stemmedContent);
                }));
            }

            await Task.WhenAll(tasks);
        }
        // **Nested Document Class**
        private class Document : IDocument
        {
            public string FilePath { get; }
            public string Content { get; set; }

            public Document(string filePath)
            {
                FilePath = filePath;
            }

            public void LoadContent()
            {
                if (File.Exists(FilePath))
                {
                    Content = File.ReadAllText(FilePath);
                }
            }
        }
    }
}
    

               
            