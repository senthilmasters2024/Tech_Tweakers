using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenAI;
using SemanticaAnalysisTextualData.Source.Enums;
public enum TextDataType
{
    Word,
    Phrase,
    Document
}
public interface IDocument
{
    string FilePath { get; }
    string Content { get; set; }
    void LoadContent();
}
public interface ITextPreprocessor
{
    /// Preprocesses text based on the specified data type (Word, Phrase, or Document).
    string PreprocessText(string text, TextDataType type);
    /// Loads documents from a specified folder.
   // List<IDocument> LoadDocuments(string folderPath);
    Task ProcessAndSaveWordsAsync(string wordsFolder); // Processes and saves preprocessed words per domain in a common file.
    Task ProcessAndSavePhrasesAsync(string phrasesFolder); // Processes and saves all preprocessed phrases in a common file.


    Task ProcessAndSaveDocumentsAsync(string inputFolder, string outputFolder);//Processes and saves all preprocessed documents in a common file.



    /// Applies stemming to the text
    string StemText(string text);

    /// Asynchronously applies stemming to all documents in a folder and saves the output.

    Task StemDocumentsInFolderAsync(string inputFolder, string outputFolder);
}