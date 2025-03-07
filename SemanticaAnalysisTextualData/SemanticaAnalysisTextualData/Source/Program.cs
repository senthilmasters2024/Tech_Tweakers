using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SemanticaAnalysisTextualData.Source.Interfaces;

namespace SemanticAnalysisTextualData.Source
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //  Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPreprocessor, TextPreprocessor>()
                .BuildServiceProvider();

            var textPreprocessor = serviceProvider.GetRequiredService<IPreprocessor>();

            //  Define Input Folders
            string sourceDomainsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\SourceBasedOnDomains";
            string sourceRelevanceFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\SourceBasedOnNeededRelevance";

            //  Define Output Folders
            string outputDomainsFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\PreprocessedSourceBasedOnDomains";
            string outputRelevanceFolder = "D:\\OPEN PROJECT HERE\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\PreprocessedSourceBasedOnNeededRelevance";

            //  Ensure output folders exist
            EnsureDirectoryExists(outputDomainsFolder);
            EnsureDirectoryExists(outputRelevanceFolder);

            //  Process both source folders
            await ProcessTextFilesInFolderAsync(textPreprocessor, sourceDomainsFolder, outputDomainsFolder);
            await ProcessTextFilesInFolderAsync(textPreprocessor, sourceRelevanceFolder, outputRelevanceFolder);

            Console.WriteLine(" Preprocessing for both source folders completed.");
        }

        static async Task ProcessTextFilesInFolderAsync(IPreprocessor textPreprocessor, string sourceFolder, string outputFolder)
        {
            Console.WriteLine($" Checking folder: {sourceFolder}");

            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($" Source folder '{sourceFolder}' not found. Skipping.");
                return;
            }

            Console.WriteLine($" Source folder exists: {sourceFolder}");


            // Ensure output folder exists
            EnsureDirectoryExists(outputFolder);

            var textFiles = Directory.GetFiles(sourceFolder, "*.txt");

            if (textFiles.Length == 0)
            {
                Console.WriteLine($" No text files found in '{sourceFolder}'. Skipping.");
                return;
            }

            Console.WriteLine($" Found {textFiles.Length} text files in '{{sourceFolder}}'. Processing...\");");

            foreach (var file in textFiles)
            {
                string originalFileName = Path.GetFileName(file);
                string newFileName = $"preprocessed_{originalFileName}";  //  Add "preprocessed_" prefix
                Console.WriteLine($" Processing file: {originalFileName} → {newFileName}");
                string fileContent = await File.ReadAllTextAsync(file);

                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    Console.WriteLine($" Skipping empty file: {originalFileName}");
                    continue;
                }

                //  Preprocess the content
                string preprocessedContent = textPreprocessor.PreprocessText(fileContent, TextDataType.Document);
                string outputFilePath = Path.Combine(outputFolder, newFileName); //  Keep the same filename

                await File.WriteAllTextAsync(outputFilePath, preprocessedContent);
                Console.WriteLine($" Preprocessed and saved: {outputFilePath}");
            }
        }

        //  Helper method to ensure a directory exists
        static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
