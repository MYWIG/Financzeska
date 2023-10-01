namespace FinanczeskaServerApp.Data
{
    public class ChatHistory
    {
        public ChatHistory(int? botType, List<Message> messages)
        {
            if(messages == null)
            {
                messages = new List<Message>();
            }

            Messages = messages;
            BotType = botType;
        }

        public List<Message> Messages { get; set; }

        public int? BotType { get; set; }
    }
}
