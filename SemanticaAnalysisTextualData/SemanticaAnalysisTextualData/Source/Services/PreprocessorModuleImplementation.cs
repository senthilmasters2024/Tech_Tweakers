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
    public async Task SaveWordsAsync(string domainName, IEnumerable<string> words, string outputFolder)
    {
        string domainPath = Path.Combine(outputFolder, "Words");
        Directory.CreateDirectory(domainPath);// Ensure the Words folder exists

        string filePath = Path.Combine(domainPath, "preprocessed_words.txt");
        await File.WriteAllLinesAsync(filePath, words);
    }

    public async Task SavePhrasesAsync(string domainName, IEnumerable<string> phrases, string outputFolder)
    {
        string outputPath = Path.Combine(outputFolder, "Phrases", domainName);
        Directory.CreateDirectory(outputPath); // ✅ Corrected

        string filePath = Path.Combine(outputPath, "preprocessed_phrases.txt"); // ✅ Corrected
        await File.WriteAllLinesAsync(filePath, phrases);
    }

    public async Task SaveDocumentsAsync(string documentType, IEnumerable<string> documents, string outputFolder)
    {
        string documentPath = Path.Combine(outputFolder, "Documents");
        Directory.CreateDirectory(documentPath);// Ensure the Documents folder exists

        string filePath = Path.Combine(documentPath, "preprocessed_documents.txt");
        await File.WriteAllLinesAsync(filePath, documents);
    }

    // Data Loading Methods
    public async Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Words", domainName, "preprocessed_words.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync(string domainName, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Phrases", domainName, "preprocessed_phrases.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    public async Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync(string documentType, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Documents", documentType, "preprocessed_documents.txt");
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
    public async Task ProcessAndSaveWordsAsync(string domainName, string wordsFolder, string outputFolder)
    {
        string domainPath = Path.Combine(wordsFolder, domainName);
        if (!Directory.Exists(domainPath))
        {
            Console.WriteLine($"Domain '{domainName}' not found in Words folder.");
            return;
        }

        var processedWords = Directory.GetFiles(domainPath, "*.txt")
            .SelectMany(file => File.ReadLines(file))
            .Select(word => PreprocessText(word, TextDataType.Word))
            .Where(word => !string.IsNullOrEmpty(word))
            .Distinct()
            .ToList();

        await SaveWordsAsync(domainName, processedWords, outputFolder);
    }

    public async Task ProcessAndSavePhrasesAsync(string domainName,string phrasesFolder, string outputFolder)
    {
        string domainPath = Path.Combine(phrasesFolder, domainName);
        if (!Directory.Exists(domainPath))
        {
            Console.WriteLine($"Domain '{domainName}' not found in Phrases folder.");
            return;
        }

        var processedPhrases = Directory.GetFiles(domainPath, "*.txt")
            .SelectMany(file => File.ReadLines(file))
            .Select(phrase => PreprocessText(phrase, TextDataType.Phrase))
            .Where(phrase => !string.IsNullOrEmpty(phrase));

        await SavePhrasesAsync(domainName, processedPhrases, outputFolder);
    }



    public async Task ProcessAndSaveDocumentsAsync(string documentType, string documentsFolder, string outputFolder)
    {
        string documentPath = Path.Combine(documentsFolder, documentType); // Select the document type folder

        if (!Directory.Exists(documentPath))
        {
            Console.WriteLine($"Document type '{documentType}' not found in Documents folder.");
            return;
        }

        var processedDocuments = Directory.GetFiles(documentPath, "*.txt")
            .Select(file => PreprocessText(File.ReadAllText(file), TextDataType.Document))
            .ToList();

        await SaveDocumentsAsync(documentType, processedDocuments, outputFolder);
    }
}