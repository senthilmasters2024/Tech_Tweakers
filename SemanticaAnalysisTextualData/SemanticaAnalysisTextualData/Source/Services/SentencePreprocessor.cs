using System.Text.RegularExpressions;
using TiktokenSharp;

public class SentencePreprocessor : ISentencePreprocessor
{
    private IWordPreprocessor wordProcessor;

    public SentencePreprocessor()
    {
        wordProcessor = new WordPreprocessor();
    }

    public List<int> ProcessSentence(string sentence)
    {
        sentence = sentence.ToLower();
        sentence = Regex.Replace(sentence, @"[^a-zA-Z0-9\s]", ""); // Remove punctuation

        string[] words = sentence.Split(' ');
        HashSet<int> uniqueTokens = new HashSet<int>();

        foreach (string word in words)
        {
            int tokenId = wordProcessor.ProcessWord(word);
            if (tokenId != -1) // Ignore stopwords
                uniqueTokens.Add(tokenId);
        }

        return uniqueTokens.ToList();
    }
}
