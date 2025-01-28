using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        string filePath = @"E:\SE PROJECT\documents\documents\Germany.txt";
        int chunkSize = 1500;


        {
            string text = File.ReadAllText(filePath);
            List<string> chunks = ChunkText(text, chunkSize);

            Console.WriteLine($"Total Chunks Created: {chunks.Count}");


            foreach (var chunk in chunks)
            {
                Console.WriteLine($"- {chunk}\n");
            }
        }
    }
{
    if (!string.IsNullOrEmpty(chunk))
            chunks.Add(chunk.Trim());

        return chunks;
    }
}
