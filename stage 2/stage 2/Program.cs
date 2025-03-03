using System;
using System.Collections.Generic;
using TiktokenSharp;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the preprocessors using their respective interfaces
        IWordPreprocessor wordPreprocessor = new WordPreprocessor();
        ISentencePreprocessor sentencePreprocessor = new SentencePreprocessor();
        IDocumentPreprocessor documentPreprocessor = new DocumentPreprocessor();

        // Step 1: Process a Single Word
        Console.Write("Enter a word: ");
        string word = Console.ReadLine();
        int wordToken = wordPreprocessor.ProcessWord(word);
        Console.WriteLine($"Word Token: {(wordToken != -1 ? wordToken.ToString() : "Ignored (stopword or empty)")}");

        // Step 2: Process a Sentence
        Console.Write("\nEnter a sentence: ");
        string sentence = Console.ReadLine();
        List<int> sentenceTokens = sentencePreprocessor.ProcessSentence(sentence);
        Console.WriteLine("Sentence Tokens: " + (sentenceTokens.Count > 0 ? string.Join(", ", sentenceTokens) : "No valid tokens"));

        // Step 3: Process a Document
        Console.Write("\nEnter a document: ");
        string document = Console.ReadLine();
        List<int> documentTokens = documentPreprocessor.ProcessDocument(document);
        Console.WriteLine("Document Tokens: " + (documentTokens.Count > 0 ? string.Join(", ", documentTokens) : "No valid tokens"));
    }
}