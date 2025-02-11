using System.Text.RegularExpressions;

public class PhrasePreprocessor : IPhrasePreprocessor
{
    private readonly IWordPreprocessor _wordProcessor;

    public PhrasePreprocessor(IWordPreprocessor wordPreprocessor)
    {
        _wordProcessor = new WordPreprocessor();
    }

    public string ProcessPhrase(string phrase)
    {
        phrase = phrase.ToLower();

        phrase = Regex.Replace(phrase, @"[^a-zA-Z0-9\s]", "");
        var words = phrase.Split(' ');
        var processedWords = words.Select(word => WordPreprocessor.preprocessWord(word)).ToList();


        // Return the processed sentences as combined strings
        return string.Join(" ", processedWords);
    }
}
    

    