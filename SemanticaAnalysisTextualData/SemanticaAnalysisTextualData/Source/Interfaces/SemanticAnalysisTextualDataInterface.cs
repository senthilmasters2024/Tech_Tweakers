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
        double CalculateSimilarity(float[] embedding1, float[] embedding2);


        //Add Additional Method Definitions Here Based on our use case and problem scenario to be implemented
    }
}
