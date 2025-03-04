public interface ITextData
{
    string Name { get; }  // Could be the file name or identifier for words/phrases
    string Content { get; }

    void LoadContent();  // Load content (for documents, from a file; for words/phrases, directly from input)
    void SaveProcessedContent(string outputFolder);  // Save processed data

}
