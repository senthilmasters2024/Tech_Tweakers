using System;
namespace Stage_1;

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
            Console.WriteLine("Words in the input text:");
            string[] words = input.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
