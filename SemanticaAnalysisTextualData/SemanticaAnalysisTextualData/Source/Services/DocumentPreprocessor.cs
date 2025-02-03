
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.Interfaces;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TiktokenSharp;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

public class DocumentPreprocessor : IDocumentPreprocessor
{
    private readonly ISentencePreprocessor _sentencePreprocessor;
    public DocumentPreprocessor(ISentencePreprocessor sentencePreprocessor)
    {
        _sentencePreprocessor = sentencePreprocessor;
    }
    //Method to load documents from a folder
    public List<IDocument> LoadDocuments(string folderPath)
    {
        List<IDocument> documents = new List<IDocument>();
        foreach (var filePath in Directory.GetFiles(folderPath, "*.txt")) // Assuming .txt files
        {
            string content = File.ReadAllText(filePath);
            documents.Add(new Document(Path.GetFileName(filePath), content));
        }
        return documents;
    }
     
   




    //Method to preprocess text
    public string PreprocessText(string text)

    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

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
    // Method to process and save documents
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

