
public class DocumentPreprocessor : IDocumentPreprocessor
{
    private readonly ISentencePreprocessor _sentencePreprocessor;
    private readonly SentencePreprocessor _sentenceProcessor;

    public DocumentPreprocessor()
    {
        _sentenceProcessor = new SentencePreprocessor();
    }

    public string ProcessTwoDocuments(string document1, string document2)
    {
        string[] sentences1 = document1.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
        string[] sentences2 = document2.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
        // Process each sentence from both documents
        var sentenceTokens1 = sentences1.Select(sentence => _sentenceProcessor.ProcessSentences(sentence.Trim(), sentence.Trim())).ToList();
        var sentenceTokens2 = sentences2.Select(sentence => _sentenceProcessor.ProcessSentences(sentence.Trim(), sentence.Trim())).ToList();

        // Return the processed documents as combined strings
        return $"{string.Join(" | ", sentenceTokens1)} | {string.Join(" | ", sentenceTokens2)}";
       
    }
}

       