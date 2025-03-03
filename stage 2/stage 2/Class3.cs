using TiktokenSharp;
public class DocumentPreprocessor : IDocumentPreprocessor
{
    private ISentencePreprocessor sentenceProcessor;

    public DocumentPreprocessor()
    {
        sentenceProcessor = new SentencePreprocessor();
    }

    public List<int> ProcessDocument(string document)
    {
        string[] sentences = document.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
        List<int> allTokens = new List<int>();

        foreach (string sentence in sentences)
        {
            List<int> sentenceTokens = sentenceProcessor.ProcessSentence(sentence.Trim());
            allTokens.AddRange(sentenceTokens);
        }

        return allTokens;
    }
}
