public interface IPreprocessor
{
    // Properties from ITextData
    string Name { get; }  // Name of the text data (e.g., file name or identifier)
    string Content { get; }  // Content of the text data
    string FilePath { get; }  // File path for documents

    // Text Preprocessing Methods
    string PreprocessText(string text, TextDataType type);
    string StemText(string text);

    // Data Storage Methods
    Task SaveWordsAsync(string domainName, IEnumerable<string> words);
    Task SavePhrasesAsync(IEnumerable<string> phrases);
    Task SaveDocumentsAsync(IEnumerable<string> documents);

    Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName);
    Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync();
    Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync();

    // Text Data Methods
    void LoadContent();  // Load content from FilePath (for documents)
    void SaveProcessedContent(string outputFolder);  // Save processed content to a file

    // Batch Processing Methods
    Task ProcessAndSaveWordsAsync(string inputFolder, string outputFolder);
    Task ProcessAndSavePhrasesAsync(string inputFolder, string outputFolder);
    Task ProcessAndSaveDocumentsAsync(string documentsFolder, string outputFolde);
}

public enum TextDataType
{
    Word,
    Phrase,
    Document
}