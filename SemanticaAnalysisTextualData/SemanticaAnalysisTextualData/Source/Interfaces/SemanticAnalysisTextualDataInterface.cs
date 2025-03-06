using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.Interfaces
{
    /// <summary>
    /// Interface for Sematic Similar Application. This is the interface that has to be invoked for using our semantica Analysis and Visualisaton
    /// </summary>
    public interface ISemanticAnalysisTextualDataInterface
    {
        /// <summary>
        /// Method which accepts simple two different texts to do a semantic similarity between them
        /// </summary>
        /// <returns></returns>
        Task PreprocessWordsAndPhrases(string wordsFolder, string phrasesFolder, string outputWords, string outputPhrases);

        /// <summary>
        /// Method to calculate similarity for words and phrases asynchronously
        /// </summary>
        /// <param name="wordEmbeddings">List of word embeddings</param>
        /// <param name="phraseEmbeddings">List of phrase embeddings</param>
        /// <returns></returns>
        Task CalculateSimilarityForWordsAndPhrasesAsync(List<double[]> wordEmbeddings, List<double[]> phraseEmbeddings);

        /// <summary>
        /// Method to preprocess all documents in the specified folders
        /// </summary>
        /// <param name="requirementsFolder">Folder containing requirement documents</param>
        /// <param name="resumesFolder">Folder containing resume documents</param>
        /// <param name="outputRequirements">Output folder for processed requirement documents</param>
        /// <param name="outputResumes">Output folder for processed resume documents</param>
        /// <returns></returns>
        Task PreprocessAllDocuments(string requirementsFolder, string resumesFolder, string outputRequirements, string outputResumes);

        /// <summary>
        /// Method to calculate similarity for documents asynchronously
        /// </summary>
        /// <param name="processedRequirementsFolder">Folder containing processed requirement documents</param>
        /// <param name="processedResumesFolder">Folder containing processed resume documents</param>
        /// <returns></returns>
        Task CalculateSimilarityForDocumentsAsync(string processedRequirementsFolder, string processedResumesFolder);

        /// <summary>
        /// Method to generate embeddings for words and phrases
        /// </summary>
        /// <param name="wordsFolder">Folder containing words</param>
        /// <param name="phrasesFolder">Folder containing phrases</param>
        /// <returns></returns>
        Task GenerateEmbeddingsForWordsAndPhrases(string wordsFolder, string phrasesFolder);

        //Add Additional Method Definitions Here Based on our use case and problem scenario to be implemented
    }
  
    
}
