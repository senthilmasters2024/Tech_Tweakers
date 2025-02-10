using System;

public interface IWordPreprocessor
{
     string ProcessWords(string word);//preprocess a single word
}

public interface IPhrasePreprocessor
{
    string PreprocessPhrase(string phrase); //preprocess a single phrase
}

public interface IDocumentPreprocessor
{
                                             
    List<IDocument> LoadDocuments(string folderPath);// Load documents from a folder
    string PreprocessDocument(string document); //Preprocess a document

    void ProcessAndSaveDocuments(string inputFolder, string outputFolder); // Process and save preprocessed documents

}
