public interface IWordPreprocessor
{
    int ProcessWord(string word);
}

public interface ISentencePreprocessor
{
    List<int> ProcessSentence(string sentence);
}

public interface IDocumentPreprocessor
{
    string ProcessTwoDocuments(string document1, string document2);
}
