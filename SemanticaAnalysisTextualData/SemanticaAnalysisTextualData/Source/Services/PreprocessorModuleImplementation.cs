using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


class TextPreprocessor : IPreprocessor
{
    // Properties from ITextData
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the content of the text data.
    /// </summary>
    public string Content { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the file path of the text data.
    /// </summary>
    public string FilePath { get; private set; } = string.Empty;

    //private readonly StanfordCoreNLP _pipeline;

    private static readonly HashSet<string> StopWords = new()
{
    "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and",
    "for", "on", "with", "but", "or", "as", "by", "from", "that", "this", "it",
    "are", "be", "was", "were", "have", "has", "had", "will", "would", "should",
    "can", "could", "which", "what", "when", "where", "who", "whom", "whose",
    "how", "why", "if", "then", "else", "while", "because", "until", "about",
    "into", "over", "under", "again", "further", "once", "here", "there", "all",
    "any", "both", "each", "few", "more", "most", "other", "some", "such", "no",
    "nor", "not", "only", "own", "same", "so", "than", "too", "very", "s", "t",
    "just", "now", "out", "up", "down", "also", "been", "being", "did", "does",
    "doing", "during", "before", "after", "above", "below", "between", "through"
};

    private static readonly Dictionary<string, string> LemmaDictionary = new()
{
    { "running", "run" },
    { "better", "good" },
    { "went", "go" },
    { "is", "be" },
    { "are", "be" },
    { "were", "be" },
    { "has", "have" },
    { "had", "have" },
    { "doing", "do" },
    { "does", "do" },
    { "did", "do" },
    // Add more mappings as needed
};
    private readonly string _storagePath;

    public TextPreprocessor()

    {
        /*
        // Initialize Stanford CoreNLP pipeline
        var props = new Properties();
        props.setProperty("annotators", "tokenize, ssplit, pos, lemma");
        _pipeline = new StanfordCoreNLP(props);

        _storagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
        Directory.CreateDirectory(_storagePath); // Ensure the storage directory exists
        */

        _storagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
        Directory.CreateDirectory(_storagePath); // Ensure the storage directory exists


    }


    /// <summary>
    /// Initialize Name, Content, and FilePath for a specific text data.
    /// </summary>
    /// <param name="name">The name of the text data.</param>
    /// <param name="content">The content of the text data.</param>
    /// <param name="filePath">The file path of the text data.</param>
    public void InitializeTextData(string name, string content, string? filePath = null)
    {
        Name = name;
        Content = content;
        FilePath = filePath ?? string.Empty;
    }

    /// <summary>
    /// Preprocesses the given text based on the specified type.
    /// </summary>
    /// <param name="text">The text to preprocess.</param>
    /// <param name="type">The type of text data (Word, Phrase, Document).</param>
    /// <returns>The preprocessed text.</returns>
    public string PreprocessText(string text, TextDataType type)
    {

        // Convert to lowercase
        text = text.ToLower().Trim();
        text = Regex.Replace(text, @"<[^>]+>|http[^\s]+", "");// // Remove HTML tags and URLs
        // Normalize contractions and abbreviations
        text = Regex.Replace(text, @"\b(can't|won't|don't|i'm|you're)\b", match =>
        {
            switch (match.Value)
            {
                case "can't": return "cannot";

                case "won't": return "will not";

                case "don't": return "do not";

                case "i'm": return "i am";

                case "you're": return "you are";

                default: return match.Value;

            }

        });
        // Remove special characters and punctuation
        text = Regex.Replace(text, @"[^a-zA-Z0-9\s'-]", "");
        // Handle numbers and dates
        //text = Regex.Replace(text, @"\b\d+\b", "number");
        //text = Regex.Replace(text, @"\b\d{4}-\d{2}-\d{2}\b", "date");
        text = LemmatizeText(text); // Apply lemmatization

        if (type == TextDataType.Word){

            return StopWords.Contains(text) ? string.Empty : text;

        }

        else if (type == TextDataType.Phrase)
{

            var words = text.Split(' ').Select(word => PreprocessText(word, TextDataType.Word));

            return string.Join(" ", words.Where(w => !string.IsNullOrEmpty(w)));

        }

        else if (type == TextDataType.Document)

        {

            // Use a better sentence tokenizer

            var sentences = Regex.Split(text, @"(?<=[.!?])\s+(?=[A-Z])");

            var processedSentences = sentences.Select(sentence => PreprocessText(sentence, TextDataType.Phrase));

            return string.Join(". ", processedSentences);

        }



        return text;

    }

    /// <summary>
    /// Lemmatize a single word using the dictionary.
    /// </summary>
    /// <param name="word">The word to lemmatize.</param>
    /// <returns>The lemmatized form of the word.</returns>
    public static string LemmatizeWord(string word)
    {
        // Check if the word exists in the lemma dictionary
        if (LemmaDictionary.TryGetValue(word, out var lemma))
        {
            return lemma; // Return the lemma if found
        }
        return word; // Return the original word if no lemma is found
    }
    /// <summary>
    /// Lemmatize an entire text using the dictionary.
    /// </summary>
    /// <param name="text">The text to lemmatize.</param>
    /// <returns>The lemmatized form of the text.</returns>
    public static string LemmatizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty; // Return empty or whitespace text as-is
        }

        // Split the text into words, lemmatize each word, and join them back
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lemmatizedWords = words.Select(LemmatizeWord);
        return string.Join(" ", lemmatizedWords);
    }
    /// <summary>
    /// Saves the preprocessed words asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="words">The words to save.</param>
    /// <param name="outputFolder">The folder to save the preprocessed words.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveWordsAsync(string domainName, IEnumerable<string> words, string outputFolder)
    {
        string domainPath = Path.Combine(outputFolder, "Words");
        Directory.CreateDirectory(domainPath);// Ensure the Words folder exists
        string filePath = Path.Combine(domainPath, "preprocessed_words.txt");
        await File.WriteAllLinesAsync(filePath, words);
    }


    /// <summary>
    /// Saves the processed phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="phrases">The phrases to save.</param>
    /// <param name="outputFolder">The folder to save the processed phrases.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SavePhrasesAsync(string domainName, IEnumerable<string> phrases, string outputFolder)
    {

        string outputPath = Path.Combine(outputFolder, "Phrases", domainName);
        Directory.CreateDirectory(outputPath); //  Corrected
        string filePath = Path.Combine(outputPath, "preprocessed_phrases.txt"); // Corrected
        await File.WriteAllLinesAsync(filePath, phrases);
    }


    /// <summary>
    /// Saves the processed documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="documents">The documents to save.</param>
    /// <param name="outputFolder">The folder to save the processed documents.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveDocumentsAsync(string documentType, IEnumerable<string> documents, string outputFolder)
    {

        string documentPath = Path.Combine(outputFolder, "Documents");

        Directory.CreateDirectory(documentPath);// Ensure the Documents folder exists



        string filePath = Path.Combine(documentPath, "preprocessed_documents.txt");

        await File.WriteAllLinesAsync(filePath, documents);

    }


    /// <summary>
    /// Loads preprocessed words asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="outputFolder">The folder to load the preprocessed words from.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the loaded preprocessed words.</returns>
    public async Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Words", domainName, "preprocessed_words.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Loads preprocessed phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="outputFolder">The folder to load the preprocessed phrases from.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the loaded preprocessed phrases.</returns>
    public async Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync(string domainName, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Phrases", domainName, "preprocessed_phrases.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Loads preprocessed documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="outputFolder">The folder to load the preprocessed documents from.</param>
    /// <returns>A task representing the asynchronous operation, with a result of the loaded preprocessed documents.</returns>
    public async Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync(string documentType, string outputFolder)
    {
        string filePath = Path.Combine(outputFolder, "Documents", documentType, "preprocessed_documents.txt");
        return File.Exists(filePath) ? await File.ReadAllLinesAsync(filePath) : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Loads the content from the file specified by the FilePath property.
    /// </summary>
    public void LoadContent()
    {
        if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
        {
            Content = File.ReadAllText(FilePath);
        }
    }

    /// <summary>
    /// Saves the processed content to the specified output folder.
    /// </summary>
    /// <param name="outputFolder">The folder to save the processed content.</param>
    public void SaveProcessedContent(string outputFolder)
    {
        if (!string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(Name))
        {
            string outputPath = Path.Combine(outputFolder, $"{Name}.txt");
            File.WriteAllText(outputPath, Content);
        }
    }

    /// <summary>
    /// Processes and saves words asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="wordsFolder">The folder containing the words.</param>
    /// <param name="outputFolder">The folder to save the processed words.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Processes and saves phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The name of the domain.</param>
    /// <param name="phrasesFolder">The folder containing the phrases.</param>
    /// <param name="outputFolder">The folder to save the processed phrases.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessAndSavePhrasesAsync(string domainName, string phrasesFolder, string outputFolder)
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



    /// <summary>
    /// Processes and saves documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="documentsFolder">The folder containing the documents.</param>
    /// <param name="outputFolder">The folder to save the processed documents.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessAndSaveDocumentsAsync(string documentType, string documentsFolder, string outputFolder)
    {
        string documentPath = Path.Combine(documentsFolder, documentType); // Select the document type folder

        if (!Directory.Exists(documentPath))
        {
            Console.WriteLine($"Document type '{documentType}' not found in '{documentsFolder}'. Skipping.");
            return;
        }

        string outputDocumentPath = Path.Combine(outputFolder, "Documents", documentType);
        Directory.CreateDirectory(outputDocumentPath); // Ensure the output folder exists

        var files = Directory.GetFiles(documentPath, "*.txt");

        if (files.Length == 0)
        {
            Console.WriteLine($" No text files found in '{documentPath}'. Skipping.");
            return;
        }

        Console.WriteLine($" Processing {files.Length} documents from {documentPath}...");

        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            string fileContent = await File.ReadAllTextAsync(file);

            if (string.IsNullOrWhiteSpace(fileContent))
            {
                Console.WriteLine($"Skipping empty document: {fileName}");
                continue;
            }

            string preprocessedContent = PreprocessText(fileContent, TextDataType.Document);
            string outputFilePath = Path.Combine(outputDocumentPath, fileName); // Save as same filename

            await File.WriteAllTextAsync(outputFilePath, preprocessedContent);
            Console.WriteLine($"Preprocessed and saved: {outputFilePath}");
        }
    }
}
