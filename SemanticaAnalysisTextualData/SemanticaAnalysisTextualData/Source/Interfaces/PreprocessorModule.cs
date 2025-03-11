public interface IPreprocessor
{
    // Properties from ITextData
    string Name { get; }  // Name of the text data (e.g., file name or identifier)
    string Content { get; }  // Content of the text data
    string FilePath { get; }  // File path for documents

    // Text Preprocessing Methods
    string PreprocessText(string text, TextDataType type);
   

    // Data Storage Methods
    Task SaveWordsAsync(string domainName, IEnumerable<string> words, string outputFolder);
    Task SavePhrasesAsync(string domainName,IEnumerable<string> phrases, string outputFolder);
    Task SaveDocumentsAsync(string documentType, IEnumerable<string> documents, string outputFolder);

    Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName, string outputFolder);
    Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync(string domainName, string outputFolder);
    Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync(string documentType, string outputFolder);

    // Text Data Methods
    void LoadContent();  // Load content from FilePath (for documents)
    void SaveProcessedContent(string outputFolder);  // Save processed content to a file

    // Batch Processing Methods
    Task ProcessAndSaveWordsAsync(string domainName, string wordsFolder, string outputFolder);
    Task ProcessAndSavePhrasesAsync(string domainName, string wordsFolder, string outputFolder);
    /// <summary>
    /// Processes and saves documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="documentsFolder">The folder containing the documents.</param>
    /// <param name="outputFolder">The folder to save the processed documents.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessAndSaveDocumentsAsync(string documentType, string documentsFolder, string outputFolder);
}

public enum TextDataType
{
    Word,
    Phrase,
    Document
}