using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySemanticAnalysisSample.FileHandling
{
    internal class CSVWriter
    {

        public static void WriteToCSV (string outputfilePath, List<string[]> similarityDataTable)
        {
            string outputDirectory = Path.GetDirectoryName(outputfilePath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            using (var writer = new StreamWriter(outputfilePath))
            {
                foreach (var row in similarityDataTable)
                {
                    writer.WriteLine(string.Join(",", row));
                }
            }
        }
    }
}
