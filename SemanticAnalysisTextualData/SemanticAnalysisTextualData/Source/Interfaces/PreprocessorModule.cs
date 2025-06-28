/// <summary>
/// Interface for preprocessing text data.
/// </summary>
namespace SemanticAnalysisTextualData.Source;


/// <summary>
/// Interface for preprocessing the text data (e.g words,phrases and documents)
/// </summary>
public interface IPreprocessor
{
    /// <summary>
    /// Gets the name of the text data (e.g., file name or identifier).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the content of the text data.
    /// </summary>
    string Content { get; }

    /// <summary>
    /// Gets the file path for documents.
    /// </summary>
    string FilePath { get; }

    /// <summary>
    /// Preprocesses the text based on the specified type.
    /// </summary>
    /// <param name="text">The text to preprocess.</param>
    /// <param name="type">The type of the text data.</param>
    /// <returns>The preprocessed text.</returns>
    string PreprocessText(string text, TextDataType type);

    /// <summary>
    /// Saves the words asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="words">The words to save.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveWordsAsync(string domainName, IEnumerable<string> words, string outputFolder);

    /// <summary>
    /// Saves the phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="phrases">The phrases to save.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SavePhrasesAsync(string domainName, IEnumerable<string> phrases, string outputFolder);

    /// <summary>
    /// Saves the documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="documents">The documents to save.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveDocumentsAsync(string documentType, IEnumerable<string> documents, string outputFolder);

    /// <summary>
    /// Loads the preprocessed words asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation, containing the preprocessed words.</returns>
    Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName, string outputFolder);

    /// <summary>
    /// Loads the preprocessed phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation, containing the preprocessed phrases.</returns>
    Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync(string domainName, string outputFolder);

    /// <summary>
    /// Loads the preprocessed documents asynchronously.
    /// </summary>
    /// <param name="documentType">The type of the document.</param>
    /// <param name="outputFolder">The output folder.</param>
    /// <returns>A task that represents the asynchronous operation, containing the preprocessed documents.</returns>
    Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync(string documentType, string outputFolder);

    /// <summary>
    /// Loads the content from the file path (for documents).
    /// </summary>
    void LoadContent();

    /// <summary>
    /// Saves the processed content to a file.
    /// </summary>
    /// <param name="outputFolder">The output folder.</param>
    void SaveProcessedContent(string outputFolder);

    /// <summary>
    /// Processes and saves words asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="wordsFolder">The folder containing the words.</param>
    /// <param name="outputFolder">The folder to save the processed words.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessAndSaveWordsAsync(string domainName, string wordsFolder, string outputFolder);

    /// <summary>
    /// Processes and saves phrases asynchronously.
    /// </summary>
    /// <param name="domainName">The domain name.</param>
    /// <param name="wordsFolder">The folder containing the phrases.</param>
    /// <param name="outputFolder">The folder to save the processed phrases.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

/// <summary>
/// Enum representing the type of text data.
/// </summary>
public enum TextDataType
{
    /// <summary>
    /// Represents a word type.
    /// </summary>
    Word,

    /// <summary>
    /// Represents a phrase type.
    /// </summary>
    Phrase,

    /// <summary>
    /// Represents a document type.
    /// </summary>
    Document
}
