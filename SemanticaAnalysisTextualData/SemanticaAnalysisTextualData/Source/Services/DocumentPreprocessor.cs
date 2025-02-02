
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

public class DocumentPreprocessor : IDocumentPreprocessor
{

    List<IDocument> documents = new List<IDocument>(); private readonly ISentencePreprocessor _sentencePreprocessor;
    foreach (var file in Directory.GetFiles(folderPath, "*.txt")) // Assuming .txt files
        {
            documents.Add(new Document(file));
        }

return documents;
}

    public string PreprocessText(string text)

    {
    // Convert to lowercase
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

