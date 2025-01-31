public interface IWordPreprocessor
{
     string ProcessWords(string word, string word2);
}

public interface ISentencePreprocessor
{
    string ProcessSentences(string sentence1, string sentence2);
}

public interface IDocumentPreprocessor
{

    List<IDocument> LoadDocuments(string folderPath);
    string PreprocessText(string text);
    void ProcessAndSaveDocuments(string inputFolder, string outputFolder);
}
