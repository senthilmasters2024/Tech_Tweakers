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
    List<int> ProcessDocument(string document);
}
