//using System;
//using System.Collections.Generic;
//using Plotly.NET;
//using Plotly.NET.TraceObjects;

//class ScatterPlotSampleForSimilarityScore
//{
//    static void Main()
//    {
//        // Sample data (X-axis: document names, Y-axis: similarity scores)
//        var documentNames = new List<string>
//        {
//            "ProfileADevOps",
//            "ProfileBDataScience",
//            "ProfileCFullStackDeveloper"
//        };

//        var similarityScores = new List<double>
//        {
//            0.4479,
//            0.3328,
//            0.6276
//        };

//        // Define domains
//        var domains = new List<string>
//        {
//            "DevOps",
//            "DataScience",
//            "FullStackDeveloper"
//        };

//        // Create scatter plot
//        var scatterPlot = Chart2D.Chart.Scatter<string, double, string>(
//            x: documentNames,          // X-axis: Document names
//            y: similarityScores,       // Y-axis: Similarity scores
//            mode: StyleParam.Mode.Markers // Scatter plot mode
//        )
//        .WithMarker(Marker.init(Color: Color.fromString("blue"), Size: 10)) // Styling
//        .WithTitle("Document Similarity Scores")
//        .WithXAxisStyle(title: Title.init(Text: "Document Name")) // Fixing the error by using Title.init
//        .WithYAxisStyle(title: Title.init(Text: "Similarity Score")); // Fixing the error by using Title.init

//        // Create bar chart for domains
//        var barChart = Chart2D.Chart.Column<string, double, string>(
//            x: domains,                // X-axis: Domains
//            y: similarityScores,       // Y-axis: Similarity scores
//            Text: documentNames        // Text labels for bars
//        )
//        .WithMarker(Marker.init(Color: Color.fromString("red"), Size: 10)) // Styling
//        .WithTitle("Domain Similarity Scores")
//        .WithXAxisStyle(title: Title.init(Text: "Domain")) // Fixing the error by using Title.init
//        .WithYAxisStyle(title: Title.init(Text: "Similarity Score")); // Fixing the error by using Title.init

//        // Combine scatter and bar charts
//        var combinedChart = Chart.Combine(new[] { scatterPlot, barChart });

//        // Display the combined chart
//        combinedChart.Show();
//    }
//}
