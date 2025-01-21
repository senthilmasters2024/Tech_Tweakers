
namespace MySemanticAnalysisSample.Embedding
{
    internal interface IEmbeddingGenerator
    {
        Task<float[]> CreateEmbedding(string text);
    }
}
