using Newtonsoft.Json;

namespace FinanczeskaServerApp.Data
{
    public class RootObject
    {

        [JsonProperty("results")]
        public List<Result> Results { get; set; } = new List<Result>();
    }
}
