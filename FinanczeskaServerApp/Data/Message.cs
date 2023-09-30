namespace FinanczeskaServerApp.Data
{
    public class Message
    {
        public Message(DateTime id, string text)
        {
            Id = id;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public DateTime Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
