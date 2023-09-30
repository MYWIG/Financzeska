namespace FinanczeskaServerApp.Data
{
    public class JsonDataSerializer
    {
        //class for serializing data to json
        public string Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        //method for serializing data to json
        public static void SerializeToJson(string id, string question, string answer)
        {
            //create a json string from the object
            var jsonDataSerializer = new JsonDataSerializer
            {
                Id = id,
                Question = question,
                Answer = answer
            };


            var json = System.Text.Json.JsonSerializer.Serialize(jsonDataSerializer);

            //if file exists, append to it
            if (System.IO.File.Exists($"Data/{id}-{DateTime.Now:yyyy-MM-dd}.json"))
            {
                System.IO.File.AppendAllText($"Data/{id}-{DateTime.Now:yyyy-MM-dd}.json", json);
                return;
            }
            else
            {
                System.IO.File.WriteAllText($"Data/{id}-{DateTime.Now:yyyy-MM-dd}.json", json);
                return;
            }
        }
    }
}
