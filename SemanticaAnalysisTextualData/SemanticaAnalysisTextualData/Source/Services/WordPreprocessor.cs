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

    public string ProcessWord(string word)
    {


        word = word.ToLower().Trim();

        word = Regex.Replace(word, @"[^a-zA-Z0-9]", "");


        return StopWords.Contains(word) ? string.Empty : word;
    }
}
    


