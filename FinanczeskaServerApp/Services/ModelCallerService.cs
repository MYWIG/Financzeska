using FinanczeskaServerApp.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace FinanczeskaServerApp.Services
{
    /// <summary>
    /// Serwis do wywoływania modelu
    /// </summary>
    public class ModelCallerService
    {
        private readonly IWebHostEnvironment _environment;

        public ModelCallerService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Parsuje plik CSV i formatuje go
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku CSV</param>
        /// <returns>Przetworzony tekst</returns>
        private string ParseCsvAndFormat(string filePath)
        {
            // Odczytuje cały tekst z pliku CSV
            string csvText = File.ReadAllText(filePath);

            // Zamienia średniki na kropki i usuwa tabulacje
            string cleanedCsv = csvText.Replace(';', '.').Replace("\t", "");

            // Zamienia znaki '\r\n' na kropki
            cleanedCsv = cleanedCsv.Replace("\r\n", ". ");

            return cleanedCsv;
        }

        /// <summary>
        /// Wysyła dane do serwera asystenta
        /// </summary>
        /// <param name="apiUrl">Adres URL API</param>
        /// <param name="jsonContent">Zawartość w formacie JSON</param>
        /// <returns>Odpowiedź od serwera</returns>
        private async Task<string> PostDataToServerAsync(string apiUrl, string jsonContent)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Definiuje nagłówki HTTP
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                    // Tworzy obiekt StringContent z danymi JSON
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Wysyła żądanie POST z danymi JSON
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        Console.WriteLine($"Błąd: {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Błąd HttpRequestException: {ex.Message}");
                    return null;
                }
            }
        }

        /// <summary>
        /// Wywołuje model asystenta
        /// </summary>
        /// <param name="userId">ID użytkownika</param>
        /// <param name="userInputData">Dane wprowadzone przez użytkownika</param>
        /// <param name="valueToSelect">Wartość do wyboru</param>
        /// <returns>Odpowiedź modelu asystenta</returns>
        public async Task<string> AskModel(string userId, string userInputData, string context)
        {
            try
            {

                //userInputData += "Me : " userInputData;
                string apiResponse = string.Empty;
                // Definiuje URL końcowe API
                string apiUrl = "http://46.170.221.65:5001/api/v1/generate";

                string dataFile;
                // Tymczasowo używane dane
                // dataFile = "data" + valueToSelect.ToString() + ".csv";

                //var dataFilePath = Path.Combine(_environment.WebRootPath, "PublicData" + "\\" + dataFile);

                try
                {

                    // Definiuje dane JSON do wysłania na serwer
                    string jsonData = $@"{{
                    ""n"": 1,
                    ""max_context_length"": 4096,
                    ""max_length"": 400,
                    ""rep_pen"": 1.1,
                    ""temperature"": 0.7,
                    ""top_p"": 0.92,
                    ""top_k"": 0,
                    ""top_a"": 0,
                    ""typical"": 1,
                    ""tfs"": 1,
                    ""rep_pen_range"": 100,
                    ""rep_pen_slope"": 0.7,
                    ""sampler_order"": [6, 0, 1, 3, 4, 2, 5],
                    ""system_prompt"": """",  
                    ""prompt"": ""[ Description : Ten czat prowadzony między tobą jako User i  botem jako FinAI  który jest polskojęzycznym asystentem finansowym który mając dane statystyczne firmy prowadzi pomoc klientom. Tylko odpowiada na pytania, robi to krótko i wyraźnie. Jeśli pytanie nie dotyczy statystyk firmy, lub odpowiednich danych nie ma w (statistical data) to bot mówi że nie może odpowiedzieć na to pytanie.\n (statistical data : {context} ) ]\n\n  User: {userInputData}"",
                    ""quiet"": true,
                    ""stop_sequence"": [""You: "", ""\nYou:"", ""\nUser: "", ""User: ""]
                }}";

                    // Wysyła żądanie POST
                    Console.WriteLine(jsonData);
                    apiResponse = await PostDataToServerAsync(apiUrl, jsonData);
                    if (apiResponse == null)
                        return "Błąd";

                    // Deserializuje odpowiedź JSON
                    RootObject rootObject = JsonSerializer.Deserialize<RootObject>(apiResponse);

                    // remove all not related single response
                    // Find the position of the prefix

                    Result lastResult = rootObject.results.Last();
                    int prefixIndex = lastResult.text.IndexOf("User:");

                    if (prefixIndex >= 0)
                        // Remove all text after the prefix
                        lastResult.text = lastResult.text.Substring(0, prefixIndex);

                    // Pobiera tekst z odpowiedzi
                    string text = lastResult.text;
                    return text;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }
                return "Błąd";
            }
            catch (Exception ex)
            {
                return "Błąd";
            }
        }
    }
}
