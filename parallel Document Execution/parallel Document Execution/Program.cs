class Program
{
    static async Task Main(string[] args)
    {
        string folderPath = @"C:\Documents"; // Change to your folder path

        List<string> documentPaths = Directory.GetFiles(folderPath, "*.txt").ToList();

        List<string> documents = await FetchDocumentsInParallelAsync(documentPaths);

        Console.WriteLine($"Fetched {documents.Count} documents.");
    }

    private static async Task<List<string>> FetchDocumentsInParallelAsync(List<string> paths)
    {
        List<string> documents = new List<string>();
        object lockObject = new object();

        await Task.Run(() =>
        {
            Parallel.ForEach(paths, path =>
            {
                string content = File.ReadAllText(path);
                lock (lockObject)
                {
                    documents.Add(content);
                }
            });
        });

        return documents;
    }
}
