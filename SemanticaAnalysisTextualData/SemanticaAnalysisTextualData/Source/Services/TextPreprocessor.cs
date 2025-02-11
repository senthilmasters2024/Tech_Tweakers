using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SemanticaAnalysisTextualData.Source.Enums;

namespace SemanticaAnalysisTextualData.Source.Services
{
    public class TextPreprocessor : ITextPreprocessor
    {
        private static readonly HashSet<string> StopWords = new()
    {
        "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and"
    };

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

        public void ProcessAndSaveDocuments(string inputFolder, string outputFolder)
        {
            var documents = LoadDocuments(inputFolder);
            foreach (var document in documents)
            {
                document.LoadContent();
                string preprocessedContent = PreprocessText(document.Content, TextDataType.Document);
                document.SaveProcessedContent(outputFolder);
            }
        }
    }
}