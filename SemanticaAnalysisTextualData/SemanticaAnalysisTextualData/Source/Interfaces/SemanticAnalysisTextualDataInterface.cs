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
        Task CalculateSimilarityForWordsAndPhrasesAsync(string outputWords, string outputPhrases);
        Task PreprocessAllDocuments(string requirementsFolder, string resumesFolder, string outputRequirements, string outputResumes);
        Task CalculateSimilarityForDocumentsAsync(string processedRequirementsFolder, string processedResumesFolder);

        //Add Additional Method Definitions Here Based on our use case and problem scenario to be implemented
    }
  
    
}
