/*using System.Collections.Generic;
using System.Threading.Tasks;
using SemanticaAnalysisTextualData.Source.Enums;

namespace SemanticaAnalysisTextualData.Source.Interfaces
{
    /// <summary>
    /// Interface for preprocessing textual data.
    /// </summary>
    public interface IPreprocessor
    {
        // Properties

        /// <summary>
        /// Gets the name of the text data.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the content of the text data.
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Gets the file path of the text data.
        /// </summary>
        string FilePath { get; }

        // Methods

        /// <summary>
        /// Initializes the text data with the specified name, content, and optional file path.
        /// </summary>
        /// <param name="name">The name of the text data.</param>
        /// <param name="content">The content of the text data.</param>
        /// <param name="filePath">The file path of the text data. Can be null.</param>
        void InitializeTextData(string name, string content, string? filePath = null);

        /// <summary>
        /// Preprocesses the specified text based on the given text data type.
        /// </summary>
        /// <param name="text">The text to preprocess.</param>
        /// <param name="type">The type of the text data (Word, Phrase, Document).</param>
        /// <returns>The preprocessed text.</returns>
        string PreprocessText(string text, TextDataType type);

        /// <summary>
        /// Stems the specified text.
        /// </summary>
        /// <param name="text">The text to stem.</param>
        /// <returns>The stemmed text.</returns>
        string StemText(string text);

        /// <summary>
        /// Saves the preprocessed words asynchronously.
        /// </summary>
        /// <param name="domainName">The domain name associated with the words.</param>
        /// <param name="words">The collection of words to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveWordsAsync(string domainName, IEnumerable<string> words);

        /// <summary>
        /// Saves the preprocessed phrases asynchronously.
        /// </summary>
        /// <param name="phrases">The collection of phrases to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SavePhrasesAsync(IEnumerable<string> phrases);

        /// <summary>
        /// Saves the preprocessed documents asynchronously.
        /// </summary>
        /// <param name="documents">The collection of documents to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveDocumentsAsync(IEnumerable<string> documents);

        /// <summary>
        /// Loads the preprocessed words asynchronously.
        /// </summary>
        /// <param name="domainName">The domain name associated with the words.</param>
        /// <returns>A task that represents the asynchronous load operation. The task result contains the collection of preprocessed words.</returns>
        Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName);

        /// <summary>
        /// Loads the preprocessed phrases asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous load operation. The task result contains the collection of preprocessed phrases.</returns>
        Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync();

        /// <summary>
        /// Loads the preprocessed documents asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous load operation. The task result contains the collection of preprocessed documents.</returns>
        Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync();

        /// <summary>
        /// Loads the content from the file path.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Saves the processed content to the specified output folder.
        /// </summary>
        /// <param name="outputFolder">The output folder where the processed content will be saved.</param>
        void SaveProcessedContent(string outputFolder);

        /// <summary>
        /// Processes and saves the words asynchronously.
        /// </summary>
        /// <param name="wordsFolder">The folder containing the words to process.</param>
        /// <param name="outputFolder">The output folder where the processed words will be saved.</param>
        /// <returns>A task that represents the asynchronous process and save operation.</returns>
        Task ProcessAndSaveWordsAsync(string wordsFolder, string outputFolder);

        /// <summary>
        /// Processes and saves the phrases asynchronously.
        /// </summary>
        /// <param name="phrasesFolder">The folder containing the phrases to process.</param>
        /// <param name="outputFolder">The output folder where the processed phrases will be saved.</param>
        /// <returns>A task that represents the asynchronous process and save operation.</returns>
        Task ProcessAndSavePhrasesAsync(string phrasesFolder, string outputFolder);

        /// <summary>
        /// Processes and saves the documents asynchronously.
        /// </summary>
        /// <param name="documentsFolder">The folder containing the documents to process.</param>
        /// <param name="outputFolder">The output folder where the processed documents will be saved.</param>
        /// <returns>A task that represents the asynchronous process and save operation.</returns>
        Task ProcessAndSaveDocumentsAsync(string documentsFolder, string outputFolder);
    }
}
*/