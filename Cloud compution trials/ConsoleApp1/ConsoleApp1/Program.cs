using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;

class Program
{
    static string[] stopwords = new string[] {
        "the", "is", "and", "in", "on", "a", "an", "for", "to", "with", "by", "of", "at", "or", "this", "that", "as"
    };

    static void Main()
    {
        string[] domains = { "health", "resumes", "sports" };
        string basePath = @"C:\YourFolderPath\"; // Replace with your base folder path

        foreach (var domain in domains)
        {
            Console.WriteLine($"\n--- Keywords in {domain.ToUpper()} Documents ---");
            string folderPath = Path.Combine(basePath, domain);
            foreach (var file in Directory.GetFiles(folderPath, "*.pdf"))
            {
                var text = ExtractTextFromPdf(file);
                var keywords = ExtractKeywords(text);
                Console.WriteLine($"\nFile: {Path.GetFileName(file)}");
                Console.WriteLine("Top Keywords: " + string.Join(", ", keywords.Take(10)));
            }
        }
    }

    static string ExtractTextFromPdf(string path)
    {
        using (PdfDocument document = PdfDocument.Open(path))
        {
            var allText = string.Join(" ", document.GetPages().Select(p => p.Text));
            return allText;
        }
    }

    static List<string> ExtractKeywords(string text)
    {
        var words = text.ToLower()
                        .Split(new char[] { ' ', '.', ',', ':', ';', '(', ')', '\n', '\r', '\"', '\'' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(word => word.Length > 3 && !stopwords.Contains(word))
                        .GroupBy(word => word)
                        .Select(g => new { Word = g.Key, Count = g.Count() })
                        .OrderByDescending(g => g.Count)
                        .Select(g => g.Word)
                        .ToList();
        return words;
    }
}
