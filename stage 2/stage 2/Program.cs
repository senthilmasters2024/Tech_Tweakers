using System;
using System.Collections.Generic;

namespace Stage_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a sentence or text:");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No text entered. Please try again.");
            }
            else
            {
                Console.WriteLine("Numeral representations of words in the input text:");

                // Assigned numerals to map text to numbers
                Dictionary<string, int> wordToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    { "one", 1 },
                    { "two", 2 },
                    { "three", 3 },
                    { "four", 4 },
                    { "five", 5 },
                    { "six", 6 },
                    { "seven", 7 },
                    { "eight", 8 },
                    { "nine", 9 },
                    { "ten", 10 }
                };

                // Spliting input into words
                string[] words = input.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (wordToNumber.TryGetValue(word, out int numeral))
                    {
                        Console.WriteLine($"{word} -> {numeral}");
                    }
                    else
                    {
                        Console.WriteLine($"{word} -> Not a numeral");
                    }
                }
            }
        }
    }
}



