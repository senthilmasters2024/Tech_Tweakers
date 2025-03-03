using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface IDataStorage
{
    void SaveWord(string word, string preprocessedWord); // Save preprocessed word
    void SavePhrase(string phrase, string preprocessedPhrase); // Save preprocessed phrase
    void SaveDocument(string documentPath, string preprocessedDocument); // Save preprocessed document

    string GetPreprocessedWord(string word); // Retrieve preprocessed word
    string GetPreprocessedPhrase(string phrase); // Retrieve preprocessed phrase
    string GetPreprocessedDocument(string documentPath); // Retrieve preprocessed document
}