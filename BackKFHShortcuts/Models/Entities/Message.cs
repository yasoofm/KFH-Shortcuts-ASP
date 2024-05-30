namespace BackKFHShortcuts.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
    }
}
