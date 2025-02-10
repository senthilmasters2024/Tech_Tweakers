public class Document : IDocument
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string Content { get; private set; }

    public Document(string filePath)
    {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
        //LoadContent();
    }

    public void LoadContent()
    {
        Content = File.ReadAllText(FilePath); // Read text content
    }

    public void SaveProcessedContent(string outputFolder)
    {
        string outputPath = Path.Combine(outputFolder, FileName);
        File.WriteAllText(outputPath, Content); // Save preprocessed content
    }
}
