# Tech_Tweakers
ML 24/25-09 Semantic Similarity Analysis of Textual Data
**Project Requirement:**
In this project, students must leverage OpenAI's GPT to create embeddings for a set of documents. The solution should utilize the OpenAI NuGet package, as demonstrated in this example: OpenAI Samples.
The primary objective of this project is to explore and quantify the semantic similarity between various levels of textual data, including words, phrases, sentences, paragraphs, and entire documents. The analysis will focus on measuring and comparing semantic relationships, leveraging examples with well-known entities and terms to highlight key insights.
Key Objectives:
1.	Word and Phrase Similarity Analysis:
o	Investigate semantic relationships between pairs of phrases, such as:
	“Angela Merkel” vs. “Government”
	“Cristiano Ronaldo” vs. “Government”
o	Highlight the variance in similarity across different domains and contexts.
2.	Document-Level Comparisons:
o	Analyze semantic similarity between documents on the same topic and those on unrelated topics.
o	Provide insights into how contextual alignment impacts semantic similarity.
3.	Demonstration and Validation:
o	Use examples involving widely recognized names, phrases, and documents to illustrate findings clearly and effectively.
o	Showcase the versatility of semantic similarity metrics across diverse scenarios.
Deliverables:
1.	Data Visualization:
o	Generate CSV files containing similarity metrics to facilitate the creation of diagrams using external tools like Microsoft Excel or Tableau.
o	Provide illustrative diagrams to validate and support the results of the analysis.
2.	Reproducible Codebase:
o	Develop a robust codebase that computes semantic similarity scores.
o	Ensure the code is well-documented and capable of exporting results in CSV format for further processing.
3.	Methodology Documentation:
o	Clearly document the methods and algorithms used for semantic similarity computation.
o	Explain the choice of metrics (e.g., cosine similarity, embeddings) and any pre-trained models or algorithms applied (e.g., Word2Vec, BERT).
=======
Semantica Analysis Textual Data

This project provides utilities for analyzing textual data, including calculating phrase similarities and saving results to CSV and JSON files.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Installation

1. Clone the repository: git clone https://github.com/senthilmasters2024/Tech_Tweakers.

2. Navigate to the project directory: cd semantica-analysis-textual-data

3. Restore the dependencies: dotnet restore

### It can also used after downloading the zip file which we published without using visual studio code for this please contact the developers
@senthilmasters2024@gmail.com and @neetu.ninnan21@gmail.com

## Usage
### Setup the Environment Variable
OPENAI_API_KEY="Give the Key"

### Run the Progam.CS as it is the entry point for the application and after running inorder to analysis developers are given with options to run the program

The purpose of processing and pre processing section is understand if its generating different scores leading to understand the contextual difference 
which we will help to and  clearly observe during the resutls analysis.
It has main entry point for the application to invoke the preprocessor,phrase and words or documents comparsion

Choose an option:
1. SemanticAnalysis without Preprocessing
2. SemanticAnalysis with Preprocessing

3. If you choose to 1,
Skipping preprocessing.
Choose an option:
1. Process Documents
2. Compare Phrases

4. If you choose to 2, After preprocessing, choose an option for further processing:
Choose an option:
1. Process Documents
2. Compare Phrases
   ,
### Preprocessing Documents

When you choose to preprocess documents, the application will:

1. Determine the project's root directory.
2. Define the data folder within the project root.
3. Define source and target folders dynamically.
4. Ensure output folders exist.
5. Process text files in the source folders and save the preprocessed content to the output folders.

### Helper Methods

- **EnsureDirectoryExists**: Ensures that a directory exists, creating it if necessary.
- **ProcessTextFilesInFolderAsync**: Processes text files in a specified folder, applying preprocessing and saving the results to an output folder.

### Example Code Snippet

Here is an example of how to invoke the document comparison process:    
### Invoke Document Comparison:

/// <summary>
/// Invokes the document comparison process with the specified arguments.
/// </summary>
/// <param name="args">The arguments for the document comparison process.</param>
public async Task InvokeDocumentComparsion(string[] args)
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
### Invoke Phrase Comparison:
SemanticSimilarityPhrasesWithInputDataSet invokePhrases = new SemanticSimilarityPhrasesWithInputDataSet(); 
await invokePhrases.InvokeProcessPhrases(args); 
Console.WriteLine("Phrases comparison completed.");

### Save Results to CSV and JSON

To save the results of phrase similarity calculations to CSV and JSON files, use the `SaveResultsPhrase` method:

var results = new List { new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 }, new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 } };
CsvHelperUtil.SaveResultsPhrase(results);

Similarity Score outputs for phrases are stroed at : \bin\Release\net9.0\data\output_datasetphrases.csv

###TESTING
Testing is done for all the methods , mainly for example sample method is given below.

Further test methods can be explored from visual studio by developers by opening the Test Project named as 
SemanticaAnalysisTextualDataTest where you can run and observe all the test results from the test explorer.

[TestMethod] public void SaveResultsPhrase_ShouldSaveResultsToCsvAndJsonFiles() { 
// Arrange var results = new List { new PhraseSimilarity 
{ Phrase1 = "phrase1", Phrase2 = "phrase2", Domain = "domain1", Context = "context1", SimilarityScore = 0.9 
}, 
new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", Domain = "domain2", Context = "context2", SimilarityScore = 0.8 
} 
};
string currentDir = Directory.GetCurrentDirectory();
string outputPathJson = Path.Combine(currentDir, "data", "output_dataset.json");
string outputPathCsv = Path.Combine(currentDir, "data", "output_datasetphrases.csv");
Directory.CreateDirectory(Path.GetDirectoryName(outputPathJson) ?? throw new ArgumentNullException(nameof(outputPathJson)));
Directory.CreateDirectory(Path.GetDirectoryName(outputPathCsv) ?? throw new ArgumentNullException(nameof(outputPathCsv)));

// Act
CsvHelperUtil.SaveResultsPhrase(results);

// Assert JSON file
Assert.IsTrue(File.Exists(outputPathJson), "JSON file should be created.");
var jsonContent = File.ReadAllText(outputPathJson);
var deserializedObj = JsonConvert.DeserializeObject<InputDataset>(jsonContent);
Assert.IsNotNull(deserializedObj);
Assert.AreEqual(results.Count, deserializedObj.PhrasePairs.Count, "The number of records in JSON should match.");

// Assert CSV file
Assert.IsTrue(File.Exists(outputPathCsv), "CSV file should be created.");
using (var reader = new StreamReader(outputPathCsv))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    var readResults = csv.GetRecords<PhraseSimilarity>().ToList();
    Assert.AreEqual(results.Count, readResults.Count, "The number of records in CSV should match.");
    for (int i = 0; i < results.Count; i++)
    {
        Assert.AreEqual(results[i].Phrase1, readResults[i].Phrase1);
        Assert.AreEqual(results[i].Phrase2, readResults[i].Phrase2);
        Assert.AreEqual(results[i].Domain, readResults[i].Domain);
        Assert.AreEqual(results[i].Context, readResults[i].Context);
        Assert.AreEqual(results[i].SimilarityScore, readResults[i].SimilarityScore);
    }
}

###Visualization

First Plot : Number of Possible Comparisons on X-axis vs Similairty Score between Documents or Phrases in Y axis

https://senthilmasters2024.github.io/Tech_Tweakers/SemanticSimilarityLatestPlot.html

Second Plot: Scalar Values in X-axis vs Similarity Score in Y-Axis (Represents between two phrases or two documents)

https://senthilmasters2024.github.io/Tech_Tweakers/ScalarValuesVsSimilarityScorePlotForOneComparsion.html


###Visualization Done Using The Help of Python

Install Pycharm and Require Dependecies from requirements.txt, Once everything is installed you can import the project as a folder 
and pointing the appropirate python file and running we will be able to plot the latest similarity scores generated from dotnet project.

# Define the relative path to your file

App2.py and FirstPlotForSimilarity.py has the latest code to plot the document and phrase comparsion charts
file_path = os.path.join(current_directory, 'data', 'output_dataset.csv') 

df = pd.read_csv("phrase_similarity.csv")

For the second chart,  in the scalarPlot1.py we can change the corresponding output csv for finding their each of their scalar values to plot against similarity score between the documents

# Read embedding values from two input CSV files
collection1 = pd.read_csv('preprocessed_JobProfileCDeveloper.txtembedding_values.csv', header=None).values.flatten()
collection2 = pd.read_csv('preprocessed_JobRequirement.txtembedding_values1.csv', header=None).values.flatten()

Individual scalar values of range of size from collection 0 to 3052 will be stored in this path for each Document

\bin\Release\net9.0 , developers can fetch the file results and use it with python program to create a chart and plot.

Example File: \bin\Release\net9.0\preprocessed_Aspirin.txtembedding_values1.csv

Similarity Score outputs are stroed at : \bin\Release\net9.0\data\output_dataset.csv

### Configuration

Ensure that your project structure includes the necessary data folders and files for preprocessing and processing. The application expects the following folder structure within the `data` directory:

- `SourceBasedOnDomains`
- `SourceBasedOnNeededRelevance`
- `PreprocessedSourceBasedOnDomains`
- `PreprocessedSourceBasedOnNeededRelevance`

These folders should contain the text files to be processed.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) 

This README.md file provides an overview of the project, instructions for installation, usage examples, 
testing instructions, and information on contributing and licensing. Adjust the content as needed to fit your specific 
project details.


