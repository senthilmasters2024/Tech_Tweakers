using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySemanticAnalysisSample.FileHandling
{
    internal class FileReader
    {
        private readonly string _filesPath;

        public FileReader(string filesPath)
        {
            _filesPath = filesPath;
        }

        public List<string> ReadDocuments()
        {
            List<string> documents = new List<string>();

            var documentFiles = Directory.GetFiles(_filesPath, "*.txt");

            foreach (var file in documentFiles)
            {
                try
                {
                    string documentContent = File.ReadAllText(file);
                    documents.Add(documentContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            return documents; //return dictionaries instead of list to get labels
        }
    }
}
