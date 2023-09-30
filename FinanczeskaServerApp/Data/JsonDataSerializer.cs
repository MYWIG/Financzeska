using Microsoft.AspNetCore.Hosting;
using System.Reflection.Metadata.Ecma335;

namespace FinanczeskaServerApp.Data
{
    public class JsonDataSerializer
    {

        private readonly IWebHostEnvironment _environment;

        public JsonDataSerializer(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        //method for serializing data to json
        public bool SerializeToJson(string id, string text)
        {
            try
            {
                if(id.Equals("::1"))
                {
                    id = "localhost";
                }

                object value =
                new
                {
                    Splitter = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"),
                    Text = text,
                };

                var json = System.Text.Json.JsonSerializer.Serialize(value);

                // create User Dir
                string dir = Path.Combine(_environment.ContentRootPath, id);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                //create Todays File
                string filePath = Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}.json") ;

                if (!File.Exists(filePath))
                    File.Create(filePath);

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the serialized string to the file
                    writer.Write(json);
                }

                ////fileStream(filePath, json);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
