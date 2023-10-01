using FinanczeskaServerApp.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
                string filenPath = Path.Combine(chatDirectory, chat + ".json");
                if (!File.Exists(filenPath))
                    return new List<Message>();
                
                FileInfo fileInfo = new FileInfo(filenPath);
                if (fileInfo.Length == 0)
                    return new List<Message>();

                string fileContent = File.ReadAllText(Path.Combine(chatDirectory,DateTime.Now.ToString("yyyy-MM-dd") + ".json"));

                // If the file contains a single string per line, you can split it into a list
                List<Message> chatHistory = JsonConvert.DeserializeObject<List<Message>>(fileContent);
                return chatHistory;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">Ip Usera</param>
        /// <returns></returns>
        public List<string> GetUserChatList(string userId)
        {
            try
            {
                // create User Dir
                string dir = Path.Combine(_webHostEnvironment.ContentRootPath, userId);

                if(!Directory.Exists(dir))
                    return new List<string>();

                List<string> files = Directory.GetFiles(dir).ToList();
                List<string> result = new List<string>();

                foreach (string file in files)
                    result.Add(Regex.Replace(file.ToLower(), ".json", string.Empty));

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
