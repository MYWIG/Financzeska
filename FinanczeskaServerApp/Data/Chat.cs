namespace FinanczeskaServerApp.Data
{
    public class Chat
    {
        public Chat(string name, DateOnly dateTime)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DateOnly = dateTime;
        }

        public string Name { get; set; }
        public DateOnly DateOnly { get; set; }

    }
}
