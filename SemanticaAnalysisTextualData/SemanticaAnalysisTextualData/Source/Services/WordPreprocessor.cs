using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


public class WordPreprocessor : IWordPreprocessor
{

    private static readonly HashSet<string> StopWords = new HashSet<string>
    {
        "the", "is", "in", "at", "of", "am", "pm", "one", "to", "a", "an", "and"
    };

    public string ProcessWords(string word1, string word2)
    {


        word1 = word1.ToLower().Trim();
        word2 = word2.ToLower().Trim();
        word1 = Regex.Replace(word1, @"[^a-zA-Z0-9]", "");
        word2 = Regex.Replace(word2, @"[^a-zA-Z0-9]", "");
       
        if (StopWords.Contains(word1) || string.IsNullOrEmpty(word1))
            word1 = string.Empty;

        if (StopWords.Contains(word2) || string.IsNullOrEmpty(word2))
            word2 = string.Empty;

        return $"{word1} | {word2}";
    }
}
    


