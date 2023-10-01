using FinanczeskaServerApp.Data;
using System.Text;
using System.Text.Json;

namespace FinanczeskaServerApp.Services
{
    // This Sevice is call model
    public class ModelCallerService
    {

        private readonly IWebHostEnvironment _environment;

        public ModelCallerService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        private string ParseCsvAndFormat(string filePath)
        {
            // Read all text from the CSV file
            string csvText = File.ReadAllText(filePath);

            // Replace semicolons with periods and remove tabulations (tab characters)
            string cleanedCsv = csvText.Replace(';', '.').Replace("\t", "");

            // Replace '\r\n' (carriage return and newline) with a dot
            cleanedCsv = cleanedCsv.Replace("\r\n", ". ");

            return cleanedCsv;
        }

        private async Task<string> PostDataToServerAsync(string apiUrl, string jsonContent)
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

        /// <summary>
        /// General ask model Implementation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="UserInputData"></param>
        /// <returns></returns>
        public async Task<string> AskModel(string userId, string userInputData)
        {
            try
            {
                string apiResponse = string.Empty;
                // Define the API endpoint URL
                string apiUrl = "http://46.170.221.65:5001/api/v1/generate";

                string dataFile;
                // mock Temporary
                int valueToSelect = 1;
                dataFile = "data" + valueToSelect.ToString() + ".csv";


                var dataFilePath = Path.Combine(_environment.WebRootPath, "PublicData"+ "\\"+dataFile);

                try
                {
                    string formattedCsv = ParseCsvAndFormat(dataFilePath);

                    string clientMessege = "Ile bezrobotnych bylo w drugim kwartale?";

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
                    ""rep_pen_range"": 100,
                    ""rep_pen_slope"": 0.7,
                    ""sampler_order"": [6, 0, 1, 3, 4, 2, 5],
                    ""system_prompt"": """",  
                    ""prompt"": ""[jesteś asystentem, nie zadajesz pytania, jesteś precyzyjny, twój temat to finansy oraz bankowość, inne tematy ignorujesz, odpowiadasz nie więcej, niż 15 słów, nie przeklinasz i jesteś grzeczny, nie razmawiasz na tematy nie związane z pytaniem, na pytania nie związane z twoim zakresem obowiązków przepraszasz. wiesz te dane: {formattedCsv}]\n\n{userInputData}"",
                    ""quiet"": true,
                    ""stop_sequence"": [""You: "", ""\nYou "", ""\nKoboldAI: ""]
                }}";

                    // Make the POST request
                    Console.WriteLine(jsonData);
                    apiResponse = await PostDataToServerAsync(apiUrl, jsonData);
                    if (apiResponse == null)
                        return "Error";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                // Define the text pattern to parse
                    
                // Use 
         

                RootObject rootObject = JsonSerializer.Deserialize<RootObject>(apiResponse);

                // Access the strongly typed data
                string text = rootObject.results[0].text;
                return text;
            }
            catch (Exception ex)
            {
                return "Error";
            }

        }

    }
}
