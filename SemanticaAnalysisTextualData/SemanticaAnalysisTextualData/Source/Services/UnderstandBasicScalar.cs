//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SemanticaAnalysisTextualData.Source.Services
//   {
//        class Program
//        {
//            // Scalar words grouped by categories
//            static readonly HashSet<string> Adjectives = new HashSet<string>
//        { "small", "medium", "large", "cold", "cool", "warm", "hot", "happy", "content", "sad" };

//            static readonly HashSet<string> Adverbs = new HashSet<string>
//        { "rarely", "sometimes", "often", "always", "slightly", "moderately", "extremely" };

//            static readonly HashSet<string> Quantifiers = new HashSet<string>
//        { "few", "some", "many", "most", "all" };

//            static void Main(string[] args)
//           /* {
//                Console.WriteLine("Enter the file path of the document:");
//                string filePath = Console.ReadLine();

//                if (!File.Exists(filePath))
//                {
//                    Console.WriteLine("File not found. Please check the path and try again.");
//                    return;
//                }

//                string text = File.ReadAllText(filePath);
//                IdentifyScalarWords(text);
//            }

//            static void IdentifyScalarWords(string text)
//            {
//                // Normalize text by splitting into words and removing punctuation
//                char[] delimiters = { ' ', '.', ',', ';', ':', '!', '?', '\n', '\r' };
//                string[] words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

//                // Store identified scalar words
//                List<(string Word, string Category)> scalarWordsFound = new List<(string, string)>();

//                foreach (string word in words)
//                {
//                    string lowerWord = word.ToLower();
//                    if (Adjectives.Contains(lowerWord))
//                    {
//                        scalarWordsFound.Add((word, "Adjective"));
//                    }
//                    else if (Adverbs.Contains(lowerWord))
//                    {
//                        scalarWordsFound.Add((word, "Adverb"));
//                    }
//                    else if (Quantifiers.Contains(lowerWord))
//                    {
//                        scalarWordsFound.Add((word, "Quantifier"));
//                    }
//                }

//                // Output results
//                if (scalarWordsFound.Any())
//                {
//                    Console.WriteLine("\nScalar Words Identified:\n");
//                    foreach (var (word, category) in scalarWordsFound)
//                    {
//                        Console.WriteLine($"{word} ({category})");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("No scalar words found in the document.");
//                }
//            }
//        }*/
//    }
