namespace SemanticAnalysisTextualData.Source
{
    /// <summary>
    /// Provides helper methods for directory operations.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Ensures that a directory exists at the specified path. If the directory does not exist, it is created.
        /// </summary>
        /// <param name="path">The path of the directory to check or create.</param>
        public static void EnsureDirectoryExists(string path)
        {
            // Check if the directory exists
            if (!Directory.Exists(path))
            {
                // Create the directory if it does not exist
                Directory.CreateDirectory(path);
            }
        }
    }
}
