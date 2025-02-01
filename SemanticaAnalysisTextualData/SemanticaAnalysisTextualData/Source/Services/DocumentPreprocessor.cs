
using System.Text.RegularExpressions;

public class DocumentPreprocessor : IDocumentPreprocessor
{
    private readonly ISentencePreprocessor _sentencePreprocessor;
    private readonly SentencePreprocessor _sentenceProcessor;

    public DocumentPreprocessor()
    {
        _sentenceProcessor = new SentencePreprocessor();
    }

    public string PreprocessText(string text)
    {
        text = text.ToLower();

        // Remove special characters
        text = Regex.Replace(text, @"[^a-zA-Z0-9\s]", "");

        // Tokenization (split by whitespace)
        string[] tokens = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        // Stopword Removal (example list)
        var stopwords = new HashSet<string> { "the", "is", "in", "at", "and", "a", "to", "of" };
        tokens = tokens.Where(word => !stopwords.Contains(word)).ToArray();

        return string.Join(" ", tokens); // Return cleaned text
    }

    public void ProcessAndSaveDocuments(string inputFolder, string outputFolder)
    {
        List<IDocument> documents = LoadDocuments(inputFolder);

        foreach (var doc in documents)
        {
            string processedContent = PreprocessText(doc.Content);
            File.WriteAllText(Path.Combine(outputFolder, doc.FileName), processedContent);
            Console.WriteLine($"Processed and saved: {doc.FileName}");
        }
    }
}

