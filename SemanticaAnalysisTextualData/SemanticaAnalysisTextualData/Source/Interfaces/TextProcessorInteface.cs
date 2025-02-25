using System;
using System.Collections.Generic;
using OpenAI;
using SemanticaAnalysisTextualData.Source.Enums;
public enum TextDataType
{
    Word,
    Phrase,
    Document
}

public interface ITextPreprocessor
{
   
    //sequentially
    
            // Get all document files (e.g., .txt, .docx, etc.)
            string[] files = Directory.GetFiles(inputFolderPath, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string filePath in files)
            {
                Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");

                // Read the content of the file (example: text file)
                string content = File.ReadAllText(filePath);

    // Call your semantic analysis method here
    ProcessDocument(content, filePath);
}

Console.WriteLine("All documents processed successfully.");
        
    {
        string folderPath = @"C:\Documents"; // Change to your folder path

    List<string> documentPaths = Directory.GetFiles(folderPath, "*.txt").ToList();

    List<string> documents = await FetchDocumentsInParallelAsync(documentPaths);

    Console.WriteLine($"Fetched {documents.Count} documents.");
    }

