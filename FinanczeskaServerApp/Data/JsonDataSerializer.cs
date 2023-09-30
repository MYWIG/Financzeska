namespace FinanczeskaServerApp.Data
{
    public class JsonDataSerializer
    {
        //method for serializing data to json
        public static void SerializeToJson(string id, string text)
        {
            object value =
            new
            {
                Splitter = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"),
                Text = text,
            };

            var json = System.Text.Json.JsonSerializer.Serialize(value);
            File.AppendAllText($"Data/{id}-{DateTime.Now:yyyy-MM-dd}.json", json);
        }
    }
}
