namespace BackKFHShortcuts.Models.Request
{
    public class OpenAIRequest
    {
        public string Model { get; set; }
        public List<MessageModel> Messages { get; set;}

    }
    public class MessageModel
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
