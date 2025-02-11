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
    string PreprocessText(string text, TextDataType dataType); // Unified preprocessing for all text types
    List<IDocument> LoadDocuments(string folderPath); // Load documents
    void ProcessAndSaveDocuments(string inputFolder, string outputFolder); // Process & save documents
}
