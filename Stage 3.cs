using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stage_1
{
    class TextPreprocessorProgram
    {
        // Funwanted character removal
        public static string Preprocess(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

           //Convert to lowercase
            input = input.ToLower();

            // Remove unwanted characters (keeping only letters)
            input = Regex.Replace(input, @"[^a-zA-Z]", "");

            // Whitespace trimming
            return input.Trim();
        }

        // converting  words to numbers
        public static void ConvertWordsToNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No text entered. Please try again.");
                return;
            }

            Console.WriteLine("\nNumeral representations of words in the input text:");

            // Defining number words
            Dictionary<string, int> wordToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "one", 1 }, { "two", 2 }, { "three", 3 },
                { "four", 4 }, { "five", 5 }, { "six", 6 },
                { "seven", 7 }, { "eight", 8 }, { "nine", 9 },
                { "ten", 10 }
            };

            // Splitting input into words 
            string[] words = Regex.Split(input, @"\W+");

            foreach (string word in words)
            {
                string processedWord = Preprocess(word);

                if (wordToNumber.TryGetValue(processedWord, out int numeral))
                    Console.WriteLine($"{word} -> {numeral}");
                else
                    Console.WriteLine($"{word} -> Not a numeral");
            }
        }

        static void Main(string[] args)
        {
           
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

            //   Words to Numbers
            Console.Write("\nEnter a sentence or text: ");
            string input = Console.ReadLine();

            ConvertWordsToNumbers(input);
        }
    }
}

