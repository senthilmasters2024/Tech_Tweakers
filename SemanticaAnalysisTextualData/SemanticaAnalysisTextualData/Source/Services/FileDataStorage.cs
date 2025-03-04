using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticaAnalysisTextualData.Source.Services
{
    public class FileDataStorage : IDataStorage
    {
        private readonly string _storagePath;

        public FileDataStorage()
        {
            _storagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data", "Output Data"));
            Console.WriteLine($"Storage Path: {_storagePath}"); // Debugging output
            Directory.CreateDirectory(_storagePath); // Ensure the storage directory exists
        }

       
            public async Task SaveWordsAsync(string domainName, IEnumerable<string> words)
        {
            string outputPath = Path.Combine(_storagePath, "Words");
            Directory.CreateDirectory(outputPath); // Ensure the subfolder exists

            string filePath = Path.Combine(outputPath, "preprocessed_words.txt");

            Console.WriteLine($"Saving words to: {filePath}"); // Debugging output

            await File.WriteAllLinesAsync(filePath, words);
            Console.WriteLine($"Successfully saved {words.Count()} words to {filePath}");
        }



        


        public async Task SavePhrasesAsync(IEnumerable<string> phrases)
        {
           
            string outputPath = Path.Combine(_storagePath, "Phrases");
            Directory.CreateDirectory(outputPath);
            string filePath = Path.Combine(outputPath, "preprocessed_phrases.txt");
            Console.WriteLine($"Saving phrases to: {filePath}"); // Debugging output
            await File.WriteAllLinesAsync(filePath, phrases);

        }


        public async Task SaveDocumentsAsync(IEnumerable<string> documents)
        {
            
            string outputPath = Path.Combine(_storagePath, "Documents");
            Directory.CreateDirectory(outputPath);
            string filePath = Path.Combine(outputPath, "preprocessed_documents.txt");
            Console.WriteLine($"Saving documents to: {filePath}"); // Debugging output
            await File.WriteAllLinesAsync(filePath, documents);


        }

        // <summary>
        /// Loads all preprocessed words for a given domain.
        /// </summary>
        public async Task<IEnumerable<string>> LoadPreprocessedWordsAsync(string domainName)
        {
            string filePath = Path.Combine(_storagePath, $"{domainName}_preprocessed_words.txt");

            if (!File.Exists(filePath))
                return Enumerable.Empty<string>(); // Return empty if file does not exist

            return await File.ReadAllLinesAsync(filePath);
        }

        /// <summary>
        /// Loads all preprocessed phrases.
        /// </summary>
        public async Task<IEnumerable<string>> LoadPreprocessedPhrasesAsync()
        {
            string filePath = Path.Combine(_storagePath, "preprocessed_phrases.txt");

            if (!File.Exists(filePath))
                return Enumerable.Empty<string>(); // Return empty if file does not exist

            return await File.ReadAllLinesAsync(filePath);
        }

        /// <summary>
        /// Loads all preprocessed documents.
        /// </summary>
        public async Task<IEnumerable<string>> LoadPreprocessedDocumentsAsync()
        {
            string filePath = Path.Combine(_storagePath, "preprocessed_documents.txt");

            if (!File.Exists(filePath))
                return Enumerable.Empty<string>(); // Return empty if file does not exist

            return await File.ReadAllLinesAsync(filePath);
        }
    }
}
