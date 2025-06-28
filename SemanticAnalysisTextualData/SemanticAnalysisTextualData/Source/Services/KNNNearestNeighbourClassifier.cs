using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticAnalysisTextualData.Source.Services
{
    /// <summary>
    /// A simple K-Nearest Neighbors (KNN) classifier using cosine similarity for textual embeddings.
    /// </summary>
    public class KNearestNeighborsClassifier
    {
        private int _k; // Number of nearest neighbors to consider for classification
        private float[][] _data; // Training data vectors (e.g., text embeddings)
        private string[] _labels; // Labels corresponding to each training vector

        /// <summary>
        /// Initializes a new instance of the classifier with a specified k.
        /// </summary>
        /// <param name="k">Number of neighbors to consider (default is 3).</param>
        public KNearestNeighborsClassifier(int k = 3)
        {
            _k = k;
        }

        /// <summary>
        /// Fits the classifier with training data and corresponding labels.
        /// </summary>
        /// <param name="data">2D array of float vectors representing training embeddings.</param>
        /// <param name="labels">Array of string labels for the training data.</param>
        public void Fit(float[][] data, string[] labels)
        {
            _data = data;
            _labels = labels;
        }

        /// <summary>
        /// Predicts the label of a new input vector based on the k-nearest neighbors.
        /// </summary>
        /// <param name="input">Input vector (e.g., text embedding of a new document).</param>
        /// <returns>Predicted label based on majority voting from k-nearest neighbors.</returns>
        public string Predict(float[] input)
        {
            // Calculate cosine similarity between input vector and all training vectors
            var distances = _data.Select((vector, index) =>
            {
                double sim = CosineSimilarity(vector, input); // Higher similarity = closer neighbor
                return (Label: _labels[index], Score: sim);
            });

            // 1. Sort by similarity in descending order (most similar first)
            // 2. Take top-k most similar vectors
            // 3. Group them by label and count occurrences
            // 4. Return the label with the highest count (majority vote)
            return distances
                .OrderByDescending(d => d.Score)
                .Take(_k)
                .GroupBy(d => d.Label)
                .OrderByDescending(g => g.Count())
                .First().Key;
        }

        /// <summary>
        /// Computes the cosine similarity between two vectors.
        /// Cosine similarity = dot(a, b) / (||a|| * ||b||)
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>Cosine similarity score between -1 and 1.</returns>
        private double CosineSimilarity(float[] a, float[] b)
        {
            double dot = 0, magA = 0, magB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];     // Dot product
                magA += a[i] * a[i];    // Magnitude of vector a
                magB += b[i] * b[i];    // Magnitude of vector b
            }

            return dot / (Math.Sqrt(magA) * Math.Sqrt(magB) + 1e-10); // Add epsilon to avoid division by zero
        }
    }
}
