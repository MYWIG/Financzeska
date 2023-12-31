﻿using FinanczeskaServerApp.Data;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace FinanczeskaServerApp.Services
{
    public class JsonDataSerializer
    {
        private readonly IWebHostEnvironment _environment;

        public JsonDataSerializer(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        //method for serializing data to json
        public bool SerializeToJson(string ip, Chat currentConversation, Message value, int botType)
        {
            try
            {
                if (ip.Equals("::1"))
                    ip = "localhost";

                ChatHistory data = new ChatHistory(botType, new List<Message>() { value });
                var json = System.Text.Json.JsonSerializer.Serialize(data);

                // create User Dir
                string dir = Path.Combine(_environment.ContentRootPath+ "Chats", ip);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Todays File
                string filePath = Path.Combine(dir, $"{currentConversation.Name}_{DateTime.Now:yyyy-MM-dd}.json");

                if (!File.Exists(filePath))
                    File.Create(filePath);

                data = new ChatHistory(botType, null);
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 0)
                    {
                        data = JsonConvert.DeserializeObject<ChatHistory>(File.ReadAllText(filePath));
                    }
                }

                // Append the new JSON string to the existing content
                data.Messages.Add(value);
                data.BotType = botType;

                // Write the updated content back to the file
                File.WriteAllText(filePath, System.Text.Json.JsonSerializer.Serialize(data));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
