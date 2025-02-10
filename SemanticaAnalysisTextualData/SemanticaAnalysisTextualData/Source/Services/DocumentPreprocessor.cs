
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Embeddings;
using SemanticaAnalysisTextualData.Source.Interfaces;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TiktokenSharp;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

public class DocumentPreprocessor : IDocumentPreprocessor
{
    private readonly IPhrasePreprocessor _phrasePreprocessor;
    public DocumentPreprocessor(IPhrasePreprocessor phrasePreprocessoror)
    {
        _phrasePreprocessor = phrasePreprocessor;
    }
    //Method to load documents from a folder
    public List<IDocument> LoadDocuments(string folderPath)

    {
        var documents = new List<IDocument>();
        foreach (var filePath in Directory.GetFiles(folderPath, "*.txt"))
        {
            documents.Add(new Document(filePath));
        }
        return documents;
    }






    //Method to preprocess text
    public string PreprocessDocument(string document)

    {
        {
            var sentences = document.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            var processedSentences = sentences.Select(sentence => _phrasePreprocessor.PreprocessPhrase(sentence)).ToList();
            return string.Join(". ", processedSentences);
        }
    }

    public void ProcessAndSaveDocuments(string inputFolder, string outputFolder)
    {
        var documents = LoadDocuments(inputFolder);
        foreach (var document in documents)
        {
            document.LoadContent();
            string preprocessedContent = PreprocessDocument(document.Content);
            document.SaveProcessedContent(outputFolder);
        }
    }
}
