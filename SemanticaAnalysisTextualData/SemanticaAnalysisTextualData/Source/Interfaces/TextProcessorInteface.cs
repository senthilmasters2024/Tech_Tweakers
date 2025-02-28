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
    /// Preprocesses text based on the specified data type (Word, Phrase, or Document).
    string PreprocessText(string text, TextDataType type);
    /// Loads documents from a specified folder.
    List<IDocument> LoadDocuments(string folderPath);
    // Call your semantic analysis method here
    Task ProcessAndSaveDocuments(string inputFolder, string outputFolder);
   


    /// Applies stemming to the text
    string StemText(string text);

    /// Asynchronously applies stemming to all documents in a folder and saves the output.

    Task StemDocumentsInFolderAsync(string inputFolder, string outputFolder);
}