using FinanczeskaServerApp.Data;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinanczeskaServerApp.Services
{
    public class ChatService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChatService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public List<Message> GetChatHistory(string userId, string chat)
        {
            try
            {

                // Specify the directory where chat history files are stored
                string chatDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, userId);
                if(!Directory.Exists(chatDirectory))
                    return new List<Message>(){ new Message(DateTime.Now, "Error", false) };

                // read value of chat chatDirectory + .json
                // Read the content of the chat file
                string fileContent = File.ReadAllText(chatDirectory);

                // If the file contains a single string per line, you can split it into a list
                List<Message> chatHistory = JsonConvert.DeserializeObject<List<Message>>(fileContent);
                var t = chatHistory;
                return chatHistory;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
