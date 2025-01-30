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
    string ProcessTwoDocuments(string document1, string document2);
}
