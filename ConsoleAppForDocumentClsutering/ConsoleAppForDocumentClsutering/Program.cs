using System;
using System.Collections.Generic;
using System.IO;
class Program
{
    static void Main()
    {
        string filePath = @"D:\OPEN PROJECT HERE\Tech_Tweakers\ConsoleAppForDocumentClsutering\ConsoleAppForDocumentClsutering\Germany.txt";
        int chunkSize = 500;

        try 
        {
            string text = File.ReadAllText(filePath);
            List<string> chunks = ChunkText(text, chunkSize);

            Console.WriteLine($"Total Chunks Created: {chunks.Count}");


            foreach (var chunk in chunks)
            {
                Console.WriteLine($"- {chunk}\n");
            }
            SaveChunksToCSV("chunked_output.csv", chunks);
            Console.WriteLine("Chunks saved to chunked_output.csv");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    public static List<string> ChunkText(string text, int chunkSize)
    {
        List<string> chunks = new List<string>();
        string[] sentences = text.Split(new[] { ". ", "? ", "! " }, StringSplitOptions.None);

        string chunk = "";
        foreach (var sentence in sentences)
        {
            if ((chunk + sentence).Length < chunkSize)
            {
                chunk += sentence + ". ";
            }
            else
            {
                chunks.Add(chunk.Trim());
                chunk = sentence + ". ";
            }
        }

    

    if (!string.IsNullOrEmpty(chunk))
            chunks.Add(chunk.Trim());

        return chunks;
    }
    public static void SaveChunksToCSV(string filename, List<string> chunks)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine("Chunk_Number,Text");
            for (int i = 0; i < chunks.Count; i++)
            {
                writer.WriteLine($"{i + 1},\"{chunks[i]}\"");
            }
        }
    }
}


