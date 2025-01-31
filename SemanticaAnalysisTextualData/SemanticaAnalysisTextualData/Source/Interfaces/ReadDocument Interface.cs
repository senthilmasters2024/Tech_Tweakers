public interface IDocument
{
    string FileName { get; }
    string FilePath { get; }
    string Content { get; }

    void LoadContent();
    void SaveProcessedContent(string outputFolder); // Save preprocessed text
}