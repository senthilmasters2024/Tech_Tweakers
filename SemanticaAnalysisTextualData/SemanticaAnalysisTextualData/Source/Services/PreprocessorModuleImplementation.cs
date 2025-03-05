using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SemanticaAnalysisTextualData.Source.Enums;
using Porter2Stemmer;


public class TextPreprocessor : IPreprocessor
{
    // Properties from ITextData
    public string Name { get; private set; }
    public string Content { get; private set; }
    public string FilePath { get; private set; }

    private static readonly HashSet<string> StopWords = new()
    {
        "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and"
    };

    private readonly EnglishPorter2Stemmer _stemmer = new();
    private readonly string _storagePath;

    public TextPreprocessor()
    {
        _storagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
        Directory.CreateDirectory(_storagePath); // Ensure the storage directory exists
    }

    // Initialize Name, Content, and FilePath for a specific text data
    public void InitializeTextData(string name, string content, string filePath = null)
    {
        Name = name;
        Content = content;
        FilePath = filePath;
    }

    // Text Preprocessing Methods
    public string PreprocessText(string text, TextDataType type)
    {
        text = text.ToLower().Trim();
        text = Regex.Replace(text, @"[^a-zA-Z0-9\s]", "");

        if (type == TextDataType.Word)
        {
            return StopWords.Contains(text) ? string.Empty : text;
        }
        else if (type == TextDataType.Phrase)
        {
            var words = text.Split(' ').Select(word => PreprocessText(word, TextDataType.Word));
            return string.Join(" ", words.Where(w => !string.IsNullOrEmpty(w)));
        }
        else if (type == TextDataType.Document)
        {
            var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var processedSentences = sentences.Select(sentence => PreprocessText(sentence, TextDataType.Phrase));
            return string.Join(". ", processedSentences);
        }

        return text;
    }

    public string StemText(string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var stemmedWords = words.Select(word => _stemmer.Stem(word).Value);
        return string.Join(" ", stemmedWords);
    }

    // Data Storage Methods
    public async Task SaveWordsAsync(string domainName, IEnumerable<string> words)
    {
        string outputPath = Path.Combine(_storagePath, "Words");
        Directory.CreateDirectory(outputPath);

        string filePath = Path.Combine(outputPath, $"{domainName}_preprocessed_words.txt");
        await File.WriteAllLinesAsync(filePath, words);
    }

    public async Task SavePhrasesAsync(IEnumerable<string> phrases)
    {
        string outputPath = Path.Combine(_storagePath, "Phrases");
        Directory.CreateDirectory(outputPath);

        string filePath = Path.Combine(outputPath, "preprocessed_phrases.txt");
        await File.WriteAllLinesAsync(filePath, phrases);
    }

    public async Task SaveDocumentsAsync(IEnumerable<string> documents)
    {
        string outputPath = Path.Combine(_storagePath, "Documents");
        Directory.CreateDirectory(outputPath);

        string filePath = Path.Combine(outputPath, "preprocessed_documents.txt");
        await File.WriteAllLinesAsync(filePath, documents);
    }

    public async Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName)
    {
        string filePath = Path.Combine(_storagePath, "Words", $"{domainName}_preprocessed_words.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync()
    {
        string filePath = Path.Combine(_storagePath, "Phrases", "preprocessed_phrases.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync()
    {
        string filePath = Path.Combine(_storagePath, "Documents", "preprocessed_documents.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    // Text Data Methods
    public void LoadContent()
    {
        if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
        {
            Content = File.ReadAllText(FilePath);
        }
    }

    public void SaveProcessedContent(string outputFolder)
    {
        if (!string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Name))
        {
            string outputPath = Path.Combine(outputFolder, $"{Name}.txt");
            File.WriteAllText(outputPath, Content);
        }
    }

    // Batch Processing Methods
    public async Task ProcessAndSaveWordsAsync(string wordsFolder, string outputFolder)
    {
        var wordFiles = Directory.GetFiles(wordsFolder, "*.txt");
        var processedWords = wordFiles
            .SelectMany(file => File.ReadLines(file))
            .Select(word => PreprocessText(word, TextDataType.Word))
            .Where(word => !string.IsNullOrEmpty(word))
            .Distinct()
            .ToList();

        await SaveWordsAsync("words", processedWords);
    }

    public async Task ProcessAndSavePhrasesAsync(string phrasesFolder, string outputFolder)
    {
        var phraseFiles = Directory.GetFiles(phrasesFolder, "*.txt");
        var processedPhrases = phraseFiles
            .SelectMany(file => File.ReadLines(file))
            .Select(phrase => PreprocessText(phrase, TextDataType.Phrase))
            .Where(phrase => !string.IsNullOrEmpty(phrase));

        await SavePhrasesAsync(processedPhrases);
    }

    public async Task ProcessAndSaveDocumentsAsync(string documentsFolder, string outputFolder)
    {
        var documentFiles = Directory.GetFiles(documentsFolder, "*.txt");
        var processedDocuments = documentFiles
            .Select(file => PreprocessText(File.ReadAllText(file), TextDataType.Document))
            .ToList();

        await SaveDocumentsAsync(processedDocuments);
    }
}