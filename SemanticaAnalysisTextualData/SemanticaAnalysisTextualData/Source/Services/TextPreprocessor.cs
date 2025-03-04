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
        private readonly IDataStorage _dataStorage;
        public TextPreprocessor(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public string PreprocessText(string text, TextDataType dataType)
        {

            text = text.ToLower().Trim();
            text = Regex.Replace(text, @"[^a-zA-Z0-9\s]", ""); // Remove special characters
            Console.WriteLine($"Preprocessing Word: {text}"); // Debugging output

            if (dataType == TextDataType.Word)
            {
                if (StopWords.Contains(text))
                {
                    Console.WriteLine($"Skipping Stopword: {text}");
                    return string.Empty;
                }
                return text;
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

        /*public List<IDocument> LoadDocuments(string folderPath)
        {
            var documents = new List<IDocument>();
            foreach (var filePath in Directory.GetFiles(folderPath, "*.txt"))
            {
                documents.Add(new Document(filePath));
            }
            return documents;
        }
        */

        /// <summary>
        /// Processes words from different domain folders and stores each domain’s words in a single file.
        /// </summary>
        public async Task ProcessAndSaveWordsAsync(string wordsFolder)
        {
            Console.WriteLine($"Processing words from folder: {wordsFolder}");

            var wordFiles = Directory.GetFiles(wordsFolder, "*.txt"); // Fetch word files directly

            if (!wordFiles.Any())
            {
                Console.WriteLine("No word files found.");
                return;
            }

            var processedWords = wordFiles
                .SelectMany(file => File.ReadLines(file))
                .Select(word => PreprocessText(word, TextDataType.Word))
                .Where(word => !string.IsNullOrEmpty(word))
                .Distinct()
                .ToList();

            Console.WriteLine($"Processed {processedWords.Count} words.");

            if (processedWords.Any())
            {
                await _dataStorage.SaveWordsAsync("words", processedWords);
            }
        }



        /// <summary>
        /// Processes all phrases and stores them in a single file.
        /// </summary>
        public async Task ProcessAndSavePhrasesAsync(string phrasesFolder)
        {
            var phraseFiles = Directory.GetFiles(phrasesFolder, "*.txt");

            var processedPhrases = phraseFiles
                .SelectMany(file => File.ReadLines(file))
                .Select(phrase => PreprocessText(phrase, TextDataType.Phrase))
                .Where(phrase => !string.IsNullOrEmpty(phrase));

            await _dataStorage.SavePhrasesAsync(processedPhrases);
        }

        /// <summary>
        /// Processes all documents, applies stemming, and stores them in a single file.
        /// </summary>
        public async Task ProcessAndSaveDocumentsAsync(string inputFolder, string outputFolder)
        {
            
                var documentFiles = Directory.GetFiles(inputFolder, "*.txt"); //  Fetches .txt files directly
                List<string> processedDocuments = new();

                foreach (var documentPath in documentFiles)
                {
                    string content = await File.ReadAllTextAsync(documentPath);
                    string preprocessedContent = PreprocessText(content, TextDataType.Document);
                    string stemmedContent = StemText(preprocessedContent);
                    processedDocuments.Add(stemmedContent);
                }

                await _dataStorage.SaveDocumentsAsync(processedDocuments);
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
            var documentFiles = Directory.GetFiles(inputFolder, "*.txt"); //  Updated to fetch all .txt files directly
            List<Task> tasks = new List<Task>();

            foreach (var documentPath in documentFiles)
            {
                tasks.Add(Task.Run(async () =>
                {
                    string content = await File.ReadAllTextAsync(documentPath);
                    string stemmedContent = StemText(content);
                    string outputFile = Path.Combine(outputFolder, Path.GetFileName(documentPath));
                    await File.WriteAllTextAsync(outputFile, stemmedContent);
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}