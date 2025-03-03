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

        public FileDataStorage(string storagePath)
        {
            _storagePath = storagePath;
            Directory.CreateDirectory(_storagePath); // Ensure the storage directory exists
        }

        public void SaveWord(string word, string preprocessedWord)
        {
            File.WriteAllText(Path.Combine(_storagePath, $"word_{word}.txt"), preprocessedWord);
        }

        public void SavePhrase(string phrase, string preprocessedPhrase)
        {
            File.WriteAllText(Path.Combine(_storagePath, $"phrase_{phrase.GetHashCode()}.txt"), preprocessedPhrase);
        }

        public void SaveDocument(string documentPath, string preprocessedDocument)
        {
            string fileName = Path.GetFileName(documentPath);
            File.WriteAllText(Path.Combine(_storagePath, fileName), preprocessedDocument);
        }

        public string GetPreprocessedWord(string word)
        {
            return File.ReadAllText(Path.Combine(_storagePath, $"word_{word}.txt"));
        }

        public string GetPreprocessedPhrase(string phrase)
        {
            return File.ReadAllText(Path.Combine(_storagePath, $"phrase_{phrase.GetHashCode()}.txt"));
        }

        public string GetPreprocessedDocument(string documentPath)
        {
            string fileName = Path.GetFileName(documentPath);
            return File.ReadAllText(Path.Combine(_storagePath, fileName));
        }
    }
}