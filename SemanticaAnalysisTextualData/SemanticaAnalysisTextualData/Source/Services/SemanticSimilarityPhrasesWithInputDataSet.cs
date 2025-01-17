using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SemanticaAnalysisTextualData.Source.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class SemanticSimilarityPhrasesWithInputDataSet
{
    static async Task Main(string[] args)
    {
        //string huggingFaceApiUrl = "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";
      /* // string apiToken = "YOUR_HUGGING_FACE_API_TOKEN";*/

        // Load predefined dataset
        string datasetPath = "C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\InputPhrasesDataSet.json";
        var dataset = JsonConvert.DeserializeObject<InputDataset>(File.ReadAllText(datasetPath));
        var services = new ServiceCollection();
        services.AddSingleton<SemanticAnalysisTextualDataService>(provider => new SemanticAnalysisTextualDataService());
        var serviceProvider = services.BuildServiceProvider();
        var textAnalysisService = serviceProvider.GetService<SemanticAnalysisTextualDataService>();
        List<PhraseSimilarity> PhrasePairs = new List<PhraseSimilarity>();
        InputDataset obj = new InputDataset();
        // Process phrase pairs
        if (null != dataset)
        {
            foreach (var pair in dataset.PhrasePairs)
            {
                try
                {
                    if (null != textAnalysisService && null!= pair.Phrase1 && null!= pair.Phrase2)
                    {
                        var similarity = await textAnalysisService.CalculateSimilarityAsync(pair.Phrase1, pair.Phrase2);
                        Console.WriteLine($"Similarity: {similarity:F4}");
                        PhraseSimilarity obj2 = new PhraseSimilarity();
                        obj2.Phrase1 = pair.Phrase1;
                        obj2.Phrase2 = pair.Phrase2;
                        obj2.Domain = pair.Domain;
                        obj2.Context = pair.Context;
                        obj2.SimilarityScore = similarity;
                        obj.PhrasePairs.Add(obj2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                //pair.SimilarityScore = await GetSimilarityScore(pair.Phrase1, pair.Phrase2, huggingFaceApiUrl, apiToken);
            }
        }

        // Process document pairs
        //foreach (var pair in dataset.DocumentPairs)
        //{
        //    pair.SimilarityScore = await GetSimilarityScore(pair.Document1, pair.Document2, huggingFaceApiUrl, apiToken);
        //}

        // Save results back to JSON
        File.WriteAllText("C:\\Users\\ASUS\\source\\repos\\Tech_Tweakers\\SemanticaAnalysisTextualData\\SemanticaAnalysisTextualData\\data\\output_dataset.json", JsonConvert.SerializeObject(obj, Formatting.Indented));
        Console.WriteLine("Results saved to output_dataset.json.");
    }


    // Models for Dataset
    class InputDataset
    {
        [JsonProperty("phrase_pairs")]
        public List<PhraseSimilarity> PhrasePairs { get; set; } = new List<PhraseSimilarity>();

        //[JsonProperty("document_pairs")]
        //public List<DocumentSimilarity> DocumentPairs { get; set; }
    }

    class PhraseSimilarity
    {
        public string? Phrase1 { get; set; }
        public string? Phrase2 { get; set; }
        public string? Domain { get; set; }
        public string? Context { get; set; }
        public double SimilarityScore { get; set; }
    }

    //class DocumentSimilarity
    //{
    //    public string Document1 { get; set; }
    //    public string Document2 { get; set; }
    //    public string Domain { get; set; }
    //    public string Topic { get; set; }
    //    public double SimilarityScore { get; set; }
    //}
}
