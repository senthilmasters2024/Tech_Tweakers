using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface IDataStorage
{

    Task SaveWordsAsync(string domainName, IEnumerable<string> words); //Saves preprocessed words for a given domain in a single file.
    Task SavePhrasesAsync(IEnumerable<string> phrases);// Saves all preprocessed phrases in a single file.
    Task SaveDocumentsAsync(IEnumerable<string> documents);// Save preprocessed document

    /// <summary>
    /// Saves all preprocessed documents in a single file.
    /// </summary>



    /// Loads all preprocessed words from a domain.
    /// </summary>
    Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName);

    /// <summary>
    /// Loads all preprocessed phrases.
    /// </summary>
    Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync();

    /// <summary>
    /// Loads all preprocessed documents.
    /// </summary>
    Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync();
}
