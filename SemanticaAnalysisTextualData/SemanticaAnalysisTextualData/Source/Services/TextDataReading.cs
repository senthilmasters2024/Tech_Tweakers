public class TextData : ITextData
{
    public string Name { get; private set; }
    public string Content { get; private set; }
    public TextDataType DataType { get; private set; }  // Enum to differentiate types

    private string FilePath;  // Used only if it's a document

    public TextData(string input, TextDataType dataType)
    {
        DataType = dataType;
        if (dataType == TextDataType.Document)
        {
            FilePath = input;
            Name = Path.GetFileName(input);
        }
        else
        {
            Name = input;
            Content = input;
        }
    }

    public void LoadContent()
    {
        if (DataType == TextDataType.Document)
        {
            Content = File.ReadAllText(FilePath);
        }
    }

    public void SaveProcessedContent(string outputFolder)
    {
        string outputPath = Path.Combine(outputFolder, $"{Name}.txt");
        File.WriteAllText(outputPath, Content);
    }
}

public enum TextDataType
{
    Word,
    Phrase,
    Document
}
