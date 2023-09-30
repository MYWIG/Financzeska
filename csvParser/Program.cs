using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace csvParser
{
    internal class Program
    {
        private static async Task Main()
        {
            // Define the API endpoint URL
            string apiUrl = "http://46.170.221.65:5001/api/v1/generate/";
            string parsedString = "Imie mojego kolegi : Jaroslav";
            string clientMessege = "Jak ma na imie mój kolega?";
            // Define the JSON data to send to the server
            string jsonData = $@"{{
                ""n"": 1,
                ""max_context_length"": 4096,
                ""max_length"": 100,
                ""rep_pen"": 1.1,
                ""temperature"": 0.7,
                ""top_p"": 0.92,
                ""top_k"": 0,
                ""top_a"": 0,
                ""typical"": 1,
                ""tfs"": 1,
                ""rep_pen_range"": 320,
                ""rep_pen_slope"": 0.7,
                ""sampler_order"": [6, 0, 1, 3, 4, 2, 5],
                ""prompt"": ""[{parsedString}]\n\n{clientMessege}"",
                ""quiet"": true,
                ""stop_sequence"": [""You: "", ""\nYou "", ""\nKoboldAI: ""]
            }}";

            // Make the POST request
            string apiResponse = await PostDataToServerAsync(apiUrl, jsonData);

            if (apiResponse != null)
            {
                Console.WriteLine(jsonData);
                Console.WriteLine("API Response:");
                Console.WriteLine(apiResponse);
            }
        }

        private static async Task<string> PostDataToServerAsync(string apiUrl, string jsonContent)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Define the content type as application/json
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                    // Create a StringContent object with the JSON data
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Send a POST request with the JSON data
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HttpRequestException: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
