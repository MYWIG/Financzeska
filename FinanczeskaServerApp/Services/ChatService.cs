using FinanczeskaServerApp.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace FinanczeskaServerApp.Services
{
    public class ChatService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChatService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public List<Message> GetChatHistory(string userId, Chat chat)
        {
            try
            {

                // Specify the directory where chat history files are stored    
                string chatDirectory = Path.Combine(_webHostEnvironment.ContentRootPath + "\\Chats", userId);
                if(!Directory.Exists(chatDirectory))
                    return new List<Message>(){ new Message(DateTime.Now, "Error", false) };

                // read value of chat chatDirectory + .json
                // Read the content of the chat file
                string filenPath = Path.Combine(chatDirectory, chat.Name + "_" + chat.DateOnly + ".json");
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
        public List<Chat> GetUserChatList(string userIp)
        {
            try
            {
                if (userIp.Equals("::1")){
                    userIp = "localhost";
                }

                // create User Dir
                string dir = Path.Combine(_webHostEnvironment.ContentRootPath+"Chats", userIp);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                List<string> files = Directory.GetFiles(dir).ToList();
                List<Chat> result = new List<Chat>();
                foreach (string file in files)
                {

                    string[] splittedString = file.Split("_");
                    var dateString = splittedString[1];
                    string format = "yyyy-MM-dd";
                    dateString = Regex.Replace(dateString.ToLower(), ".json", string.Empty);
                    if (!DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        return result;

                    

                    Chat chat = new(splittedString[0], DateOnly.FromDateTime(parsedDate));

                    result.Add(chat);

                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task CreateNewChat(string userIp)
        {
            try
            {
                if (userIp.Equals("::1"))
                    userIp = "localhost";

                // create User Dir
                string dir = Path.Combine(_webHostEnvironment.ContentRootPath + "Chats", userIp);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Todays File
                string filePath = Path.Combine(dir, $"{Guid.NewGuid()}_{DateTime.Now:yyyy-MM-dd}.json");

                if (!File.Exists(filePath))
                    File.Create(filePath);
            }
            catch(Exception ex )
            {
                return;
            }
        }



    }
}
