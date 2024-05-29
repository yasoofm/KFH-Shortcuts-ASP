namespace BackKFHShortcuts.Models.Request
{
    public class AddRewardRequest
    {
        public string Title { get; set; }
        public int RequiredPoints { get; set; }
        public int Usages { get; set; }
        public DateTime DueDate { get; set; }
    }
}
