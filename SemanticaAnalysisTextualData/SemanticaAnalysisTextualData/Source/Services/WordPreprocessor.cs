using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TiktokenSharp; // TikToken NuGet package

public class WordPreprocessor : IWordPreprocessor
{
    private TikToken tikToken;
    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and"
    };

    public WordPreprocessor()
    {
        tikToken = TikToken.GetEncoding("cl100k_base"); // Tokenizer initialization
    }

    public int ProcessWord(string word)
    {
        word = word.ToLower().Trim();
        word = Regex.Replace(word, @"[^a-zA-Z0-9]", ""); // Remove special characters

        if (StopWords.Contains(word) || string.IsNullOrEmpty(word))
            return -1; // Return -1 for ignored words

        List<int> tokenIds = tikToken.Encode(word);
        return tokenIds.Count > 0 ? tokenIds[0] : -1; // Return first token ID
    }
}
