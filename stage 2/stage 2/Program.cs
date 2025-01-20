using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stage_1
{
    class TextPreprocessorProgram
    {
        // Function to preprocess text (remove unwanted characters)
        public static string Preprocess(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<string>();

            // Convert to lowercase
            input = input.ToLower();

            // Remove unwanted characters (keeping only letters)
            input = Regex.Replace(input, @"[^a-zA-Z]", "");

            // Trim whitespace
            return new List<string>(Regex.Split(input, "\\s+").Where(word => !string.IsNullOrEmpty(word)));
        }

        // Function to map words to numbers
        public static void ConvertWordsToNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No text entered. Please try again.");
                return;
            }

            Console.WriteLine("\nNumeral representations of words in the input text:");

            // Dictionary for number words
            Dictionary<string, int> wordToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int nextId = 1;

            List<string> words = Preprocess(input);

            foreach (string word in words)
            {
                if (!wordToNumber.ContainsKey(word))
                {
                    wordToNumber[word] = nextId;
                    nextId++;
                }
                Console.WriteLine($"{word} -> {wordToNumber[word]}");
            }
        }
        static void Main(string[] args)
        {
            // Step 1: Compare Two Words After Preprocessing
            Console.Write("Enter first word: ");
            string word1 = Console.ReadLine();

            Console.Write("Enter second word: ");
            string word2 = Console.ReadLine();

            string processedWord1 = Preprocess(word1);
            string processedWord2 = Preprocess(word2);

            Console.WriteLine("\nProcessed Word 1: " + processedWord1);
            Console.WriteLine("Processed Word 2: " + processedWord2);

            if (processedWord1 == processedWord2)
                Console.WriteLine("The words are identical after preprocessing.");
            else
                Console.WriteLine("The words are different after preprocessing.");

            // Step 2: Convert Words to Numbers
            Console.Write("\nEnter a sentence or text: ");
            string input = Console.ReadLine();

            ConvertWordsToNumbers(input);
        }
    }
}

