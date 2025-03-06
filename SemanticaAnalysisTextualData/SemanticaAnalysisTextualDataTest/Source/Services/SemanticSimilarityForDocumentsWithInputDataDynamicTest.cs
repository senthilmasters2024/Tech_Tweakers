
[TestClass]
public class SemanticSimilarityForDocumentsWithInputDataDynamicTest
{

    [TestMethod]
    static async Task TestReadInputFiles(string sourceFolder, string targetFolder)
    {
        try
        {
            var sourceFiles = Directory.GetFiles(sourceFolder, "*.txt");
            var targetFiles = Directory.GetFiles(targetFolder, "*.txt");

            Console.WriteLine("Testing reading input files...");

            foreach (var sourceFile in sourceFiles)
            {
                string fileName = Path.GetFileName(sourceFile);
                string content = await File.ReadAllTextAsync(sourceFile);
                Console.WriteLine($"Source File: {fileName}");
                Console.WriteLine($"Content: {content}");
            }

            foreach (var targetFile in targetFiles)
            {
                string fileName = Path.GetFileName(targetFile);
                string content = await File.ReadAllTextAsync(targetFile);
                Console.WriteLine($"Target File: {fileName}");
                Console.WriteLine($"Content: {content}");
            }

            Console.WriteLine("Finished testing reading input files.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading input files: {ex.Message}");
        }
    }
}