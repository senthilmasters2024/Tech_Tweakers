using System;
using System.Collections.Generic;
using SemanticaAnalysisTextualData.Source.Enums;
public enum TextDataType
{
    Word,
    Phrase,
    Document
}

public interface ITextPreprocessor
{
    //additional

    private static async Task<List<string>> FetchDocumentsInParallelAsync(List<string> paths)
    {
        List<string> documents = new List<string>();
        object lockObject = new object();

        await Task.Run(() =>
        {
            Parallel.ForEach(paths, path =>
            {
                string content = File.ReadAllText(path);
                lock (lockObject)
                {
                    documents.Add(content);
                }
            });
        });
        string PreprocessText(string text, TextDataType dataType); // Unified preprocessing for all text types
        List<IDocument> LoadDocuments(string folderPath); // Load documents
        void ProcessAndSaveDocuments(string inputFolder, string outputFolder); // Process & save documents
    }

    {
        string folderPath = @"C:\Documents"; // Change to your folder path

    List<string> documentPaths = Directory.GetFiles(folderPath, "*.txt").ToList();

    List<string> documents = await FetchDocumentsInParallelAsync(documentPaths);

    Console.WriteLine($"Fetched {documents.Count} documents.");
    }

