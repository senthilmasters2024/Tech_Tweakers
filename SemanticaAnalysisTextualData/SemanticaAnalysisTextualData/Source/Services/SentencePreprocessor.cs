using System.Text.RegularExpressions;

public class SentencePreprocessor : ISentencePreprocessor
{
    private readonly IWordPreprocessor _wordProcessor; 
  
    public SentencePreprocessor()
    {
        _wordProcessor = new WordPreprocessor();
    }

    public string ProcessSentences(string sentence1, string sentence2)
    {
        sentence1 = sentence1.ToLower();
        sentence2 = sentence2.ToLower();
        sentence1 = Regex.Replace(sentence1, @"[^a-zA-Z0-9\s]", "");
        sentence2 = Regex.Replace(sentence2, @"[^a-zA-Z0-9\s]", "");
        // Split the sentences into words
        string[] words1 = sentence1.Split(' ');
        string[] words2 = sentence2.Split(' ');
        // Process each word using the word preprocessor
        var processedWords1 = words1.Select(word => _wordProcessor.ProcessWords(word, word)).ToList();
        var processedWords2 = words2.Select(word => _wordProcessor.ProcessWords(word, word)).ToList();
        // Return the processed sentences as combined strings
        return $"{string.Join(" ", processedWords1)} | {string.Join(" ", processedWords2)}";
        }
    }
    