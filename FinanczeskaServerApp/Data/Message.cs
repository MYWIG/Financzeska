namespace FinanczeskaServerApp.Data
{
    public class Message
    {
        public Message(DateTime id, string text, bool isRequest)
        {
            Id = id;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IsRequest = isRequest;
        }

        public DateTime Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRequest { get; set; }
    }
}
