using System.Text.RegularExpressions;

namespace FinanczeskaServerApp.Services
{
    public class ChatHistoryService
    {
        private readonly IWebHostEnvironment _environment;

        public ChatHistoryService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
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
                string dir = Path.Combine(_environment.ContentRootPath, userId);

                List<string> files = Directory.GetFiles(dir).ToList();
                List<string> result = new List<string>(); 
                
                foreach(string file in files)
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
