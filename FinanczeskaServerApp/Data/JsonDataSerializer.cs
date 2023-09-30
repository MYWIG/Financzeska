using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace FinanczeskaServerApp.Data
{
    public class JsonDataSerializer
    {
        //class for serializing data to json
        public DateTime Date { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        //method for serializing data to json
        public static void SerializeToJson(string question, string answer, HttpContext httpContext)
        {
            //create a json string from the object
            var jsonDataSerializer = new JsonDataSerializer
            {
                Date = DateTime.Now,
                Question = question,
                Answer = answer
            };


            if (httpContext == null)
            {
                httpContext = new DefaultHttpContext();
            }

            string id = httpContext.Connection.RemoteIpAddress?.ToString();
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
