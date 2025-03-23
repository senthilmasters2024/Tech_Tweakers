# Tech_Tweakers
# **Semantica Analysis of Textual Data**

This project provides utilities for analyzing textual data, calculating phrase similarities, and saving results to CSV and JSON files.

## **Table of Contents**
- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Visualization](#visualization)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## **Installation**

1. Clone the repository:
   ```sh
   git clone https://github.com/senthilmasters2024/Tech_Tweakers
   ```
2. Navigate to the project directory:
   ```sh
   cd semantica-analysis-textual-data
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Alternatively, the project can be used by downloading the published zip file without Visual Studio. For this, please contact the developers at:
   - senthilmasters2024@gmail.com
   - neetu.ninnan21@gmail.com

## **Usage**

### **Setup the Environment Variable**
```sh
OPENAI_API_KEY="Your_API_Key_Here"
```

### **Run the Program**
Run `Program.cs`, which serves as the entry point for the application. Developers are given options to analyze the data:

1. **Semantic Analysis without Preprocessing**
2. **Semantic Analysis with Preprocessing**

If option 1 is chosen:
- Skip preprocessing.
- Choose:
  1. Process Documents
  2. Compare Phrases

If option 2 is chosen:
- Perform preprocessing, then choose:
  1. Process Documents
  2. Compare Phrases

### **Preprocessing Documents**
When preprocessing is selected, the application will:
1. Determine the project's root directory.
2. Define the data folder within the project root.
3. Define source and target folders dynamically.
4. Ensure output folders exist.
5. Process text files in the source folders and save the preprocessed content to the output folders.

### **Helper Methods**
- **EnsureDirectoryExists**: Ensures that a directory exists, creating it if necessary.
- **ProcessTextFilesInFolderAsync**: Processes text files in a specified folder, applying preprocessing and saving the results to an output folder.

### **Example Code Snippet**

#### **Invoke Document Comparison**
```csharp
public async Task InvokeDocumentComparison(string[] args)
{
    var serviceProvider = ConfigureServices();
    var textAnalysisService = serviceProvider.GetService<SemanticSimilarityForDocumentsWithInputDataDynamic>();

    if (textAnalysisService != null)
    {
        try
        {
            var (sourceFiles, targetFiles) = GetSourceAndTargetFiles(bool isPreProcessRequiredFlag);
            var results = await CompareDocumentsAsync(sourceFiles, targetFiles);
            CsvHelperUtil.SaveResultsToCsv(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

#### **Invoke Phrase Comparison**
```csharp
SemanticSimilarityPhrasesWithInputDataSet invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet();
await invokePhrases.InvokeProcessPhrases(args);
Console.WriteLine("Phrases comparison completed.");
```

#### **Save Results to CSV and JSON**
```csharp
var results = new List<PhraseSimilarity> {
    new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 },
    new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 }
};
CsvHelperUtil.SaveResultsPhrase(results);
```
Similarity Score outputs for phrases are stored at:
```
/bin/Release/net9.0/data/output_datasetphrases.csv
```

## **Testing**
Testing is performed for all methods. Example test:
```csharp
[TestMethod]
public void SaveResultsPhrase_ShouldSaveResultsToCsvAndJsonFiles()
{
    // Arrange
    var results = new List<PhraseSimilarity> {
        new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 },
        new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 }
    };
    CsvHelperUtil.SaveResultsPhrase(results);
    Assert.IsTrue(File.Exists("data/output_datasetphrases.csv"));
}
```

## **Visualization**
- **First Plot:** Number of Possible Comparisons (X-axis) vs. Similarity Score (Y-axis)
  - [View Plot](https://senthilmasters2024.github.io/Tech_Tweakers/SemanticSimilarityLatestPlot.html)
- **Second Plot:** Scalar Values (X-axis) vs. Similarity Score (Y-axis)
  - [View Plot](https://senthilmasters2024.github.io/Tech_Tweakers/ScalarValuesVsSimilarityScorePlotForOneComparsion.html)

## **Configuration**
Ensure the project structure includes the necessary folders:
```
- data/
  - SourceBasedOnDomains/
  - SourceBasedOnNeededRelevance/
  - PreprocessedSourceBasedOnDomains/
  - PreprocessedSourceBasedOnNeededRelevance/
```

## **Contributing**
Contributions are welcome! Open an issue or submit a pull request.

## **License**
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
