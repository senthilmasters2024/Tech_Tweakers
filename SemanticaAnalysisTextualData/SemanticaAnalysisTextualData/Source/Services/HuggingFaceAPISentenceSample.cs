using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class HuggingFaceAPISentenceSample
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        // Replace with your actual Hugging Face API token
        var apiKey = "myApiKey";

        // JSON payload for the sentences
        var json = @"{
            ""inputs"": {
                ""source_sentence"": ""I like Programming and there are lot of opporunities for earning"",
                ""sentences"": [
                    ""Programming is best choice for future job prospects"",
                    ""Some Students never like progamming""
                ]
            }
        }";

        // Create the request content
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Set the authorization header
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        // Make the POST request to Hugging Face
        var response = await client.PostAsync("https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2", requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + responseString);
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
            var errorString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Details: " + errorString);
        }
    }
}