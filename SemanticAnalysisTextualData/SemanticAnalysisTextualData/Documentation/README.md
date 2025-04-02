# Tech_Tweakers
# **Semantic Similarity Analysis of Textual Data**

## Introduction
Developed a scalable framework for Semantic Similarity Analysis of Textual Data focusing on words/phrases and/or documents comparisons using OpenAI embeddings and Cosine Similarity algorithm. The tool offers flexible preprocessing, ensuring contextually relevant similarity assessments. It demonstrates effectiveness in capturing semantic nuances with practical applications including resume filtering, admission categorization, and content classification. This project provides utilities for analyzing textual data, calculating phrase similarities, and saving results to CSV and JSON files.

### **Technologies Used:**
- C# (.NET 9.0 or Latest)
- OpenAI API (For generating embeddings)
- Python (For visualization and analysis)
- Visual Studio & Visual Studio Test Explorer

### **Key Features:**
- Compare documents and phrases with or without preprocessing.
- Generate similarity scores and visualize results using various plots.
- Save results as CSV for further analysis.
- Provide user-defined thresholds for classification.

## **Table of Contents**
- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Visualization](#visualization)
- [Configuration](#configuration)
- [Contributing](#contributing)

## **Installation**

1. Clone the repository:
   ```sh
   git clone https://github.com/senthilmasters2024/Tech_Tweakers
   ```
2. Navigate to the project directory:
   ```sh
   cd SemanticAnalysisTextualData
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Alternatively, download the published zip file from the [Release Section](https://github.com/senthilmasters2024/Tech_Tweakers/releases/tag/v1.0-beta.1).
5. For Setup and Run the application, you can have a look short setup tutorial
[SetupTutorial](https://github.com/senthilmasters2024/Tech_Tweakers/blob/main/SemanticAnalysisTextualData/SemanticAnalysisTextualData/Documentation/SetupandRunFromVisualStudioAndFromReleaseSection.mp4)

**Tools Required:**
- Visual Studio
- .NET 9.0 or Latest Installed
- Python Environment for Visualization
- Basic GitHub Knowledge

## **Usage**
### **Setup the Environment Variable**

OPENAI_API_KEY="Your_API_Key_Here" and 
How to Setup Tutorial Video can be found here -[HowtoSetOpenAPIKeyinSystemEnvironmentVariable](https://www.youtube.com/watch?v=hgXJia5WxtY)

### **Running the Program**
1. Build the solution in Visual Studio by selecting `Build > Build Solution`.
2. Run the program by pressing `F5` or using the terminal:

### **Running from Downloaded Release**
You can run the program by executing `SemanticaAnalysisTextualData.exe` via double-click or from the command prompt.

## **Before Running For Better Understanding of the Application**
You can go through our Documentation,Presentation Video and FlowChart for better understanding of the process.  
-  [Documentation Section](https://github.com/senthilmasters2024/Tech_Tweakers/tree/main/SemanticAnalysisTextualData/SemanticAnalysisTextualData/Documentation)
-  [Flow Chart](https://github.com/senthilmasters2024/Tech_Tweakers/blob/main/SimilarityFlowChart.svg) 
-  [Research Paper](https://github.com/senthilmasters2024/Tech_Tweakers/tree/main/SemanticAnalysisTextualData/SemanticAnalysisTextualData/Documentation/SemanticSimilarityAnalysisTextualData.pdf)
  
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
    var results = new List<PhraseSimilarity> {
        new PhraseSimilarity { Phrase1 = "phrase1", Phrase2 = "phrase2", SimilarityScore = 0.9 },
        new PhraseSimilarity { Phrase1 = "phrase3", Phrase2 = "phrase4", SimilarityScore = 0.8 }
    };
    CsvHelperUtil.SaveResultsPhrase(results);
    Assert.IsTrue(File.Exists("data/output_datasetphrases.csv"));
}
```

### **How To Run the Tests**
- **Using Visual Studio:** Open Test Explorer and click "Run All".
- **Using Command Line:** Run `dotnet test` in the project root folder.


### **Visualization**

You can click or hover over the dots plotted to view details of the plot like which two files are compared and corresponding similarity score
- **First Plot:** Number of Possible Comparisons (X-axis) vs. Similarity Score (Y-axis)
  - [View Plot](https://senthilmasters2024.github.io/Tech_Tweakers/SemanticSimilarityLatestPlot.html)
First Plot Helps to Understand that user or developers to observe the number of documens used for the comparsion, as each and every documents
are indivudally compared though its relavant or irrelavant which helps to study the contextual alignment more clearly.
- **Second Plot:** Scalar Values (X-axis) vs. Similarity Score (Y-axis)
  - [View Plot](https://senthilmasters2024.github.io/Tech_Tweakers/ScalarValuesVsSimilarityScorePlotForOneComparsion.html)
Second Plot Helps to Understand that user or developers to observe how you can utilize this plot to understand how contextual alignment is done
by Open AI Embedding as we trying to plot 0-3052 collections of embedding in x-axis vs Similarity Score in Y-axis, You can observe how in the different
ranges plot are travelling and varying together within that 0-3052 range between any two documents generated Embeddings  
  
- **Third Plot:** Phrases Comparsion (X-axis) vs. Similarity Score (Y-axis)
  - [View Plot](https://senthilmasters2024.github.io/Tech_Tweakers/PhrasesSimilarityClassficationByDomainsPlots.html)
Third Plot Helps to Understand the user or developers to observe the number of Phrases used for the comparsion, as each and every documents
are indivudally compared though its relavant or irrelavant which helps to study the contextual alignment more clearly.

**What we can infer from Third Plot:**
The output CSV generated from the document comparison analysis is being updated to include domain names derived from filenames, allowing for more structured, programmatic domain-based classification. This enhancement enables our approach to classify documents across different domains more effectively.
Currently, document classification is performed by comparing documents from various domains with an initial dataset, which includes categories such as medical and job requirements. In addition to this, we now classify documents based on filenames to identify their type.
For real-time applications, the system can be configured to restrict user inputs to specific file types based on predefined domain constraints. Alternatively, the application can be modified to accept datasets from a single domain (e.g., job requirements) to assess document relevance within that specific context.
If a document's domain cannot be determined due to an unexpected filename, it will be categorized as "unknown." These documents will be handled separately in the visualization, while the remaining classified documents will be analyzed within their respective domains. This approach serves as an initial filtering mechanism to enhance the clarity and interpretability of the visualization.
Through this process, we successfully achieve classification based on both domain and relevance while incorporating semantic analysis capabilities.

## **Configuration**
## **Configuration For Phrase Analysis**
- The input file `*_phrase_pairs.csv` is predefined in the data folder (`./data/sample_phrase_pairs.csv`).
- Currently supports analyzing 50 phrases.
- Results are plotted as described in the visualization section.
- You can add more files in the similar name format Ex: Sample1_phrase_pairs.csv to add your own for more analysis or else you can edit the existing file with same file name ends with format *_phrase_pairs.csv`

### **Sample CSV Structure:**
```csv
Phrase1,Phrase2,Domain,Context
Angela Merkel,Government,Politics,Leadership
Cristiano Ronaldo,Government,Sports,Irrelevant
Dog,Cat,Animals,Pets
Medicine,Paracetamol,Medical,Medical
```
## **Configuration For Document Analysis**
- Ensure the project structure includes the following folders:
```
- data/
  - SourceBasedOnDomains/                  ------ Folder is to accept the source document for comaparison
  - SourceBasedOnNeededRelevance/          ------ Folder is to accept the target document to be compared
  - PreprocessedSourceBasedOnDomains/      ------ Internally use by application for pre processing source document
  - PreprocessedSourceBasedOnNeededRelevance/  ------ Internally use by application for pre processing target document
  ```
  
### **Developer Tips for Adding New Documents for Analysis**

a.) If you using the cloned repository, you can place the new documents to be compared inside Project root and place two different documents to be
compared inside SourceBasedOnDomains and / and SourceBasedOnNeededRelevance/

b.) If you are just the user who is using the application, you can place the new documents to be compared in your local asset downloaded path
SemanticAnalysisTextualData\data\SourceBasedOnDomains\ and SemanticAnalysisTextualData\data\SourceBasedOnNeededRelevance\

## **Save Results to CSV and Compared Analysis Scores can be Found at Path**
```
Phrase Comparison Output  CSV Path - *.\data\output_datasetphrases.csv
Document Comparison Output CSV Path - *.\data\output_dataset.csv

If you want to See the Results of the Embedding generated at the range of 0-3052 it can be found data\fileName.csv , filename may either be appended with
preprocessed_JobProfileCDeveloper.txtembedding_values or JobProfileCDeveloper.txtembedding_values depends on the choice you used for the analysis-textual-data
```
#### **How to Plot the Score Using Python**

Python codes used for Plotting can be found at:
- [PythonCodeForVisualisation](https://github.com/senthilmasters2024/Tech_Tweakers/tree/main/SemanticAnalysisTextualData/PythonCodeForVisualisation)
For Phrase Similarity - PhraseandWordsSimilarityPlot.py
For Document Similarity - DocumentAnalysisVisualisationLatest.py
For Plotting Similarity Score with Scalar Values - SimilarityPlotWithScalarValues.py
For Downloading Required Libs - requirements.txt
- [YoutubeLinkForBasicSetupForPython](https://www.youtube.com/watch?v=GK0usm20xes)
You can create a new python project and install required dependecies using requirement.txt which is available inside the source repository

Inorder to execute the code, you must copy the generated output data folder to the Python Project Root and then if you execute

file_path = os.path.join(current_directory, 'data', 'output_dataset.csv')

With the help of this code,it will pick the latest similarity score and plot the scores

Currently we are using externally to plot and developer should be able to understand how it is plotted, we are looking to improve it by integrating with some 
kind of automatic scripts and currently implementation may roll out in future.
#### **Limitations**

Currently we are supporting only text files for analysis of documents, By improving the solution by introducing future plan is
to integrate to support docx and pdf files also to support the enhanced version where then this application can be used 
for real time applications like Resume Filtering and Admission Requirement Matching.

#### **Overcoming Limitations**

Future Release will be focussed to cover all this drawbacks and make the application better.

## **Contributing**

Contributions are welcome! To contribute:
- Fork the repository.
- Create a new branch (feature/your-feature-name).
- Make your changes and test them.
- Submit a pull request with a detailed explanation of your changes.
