public interface IDocument
{
    string FileName { get; }
    string FilePath { get; }
    string Content { get; }

    void LoadContent(); // Load document content
    void SaveProcessedContent(string outputFolder); // Save preprocessed text
}