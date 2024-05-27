namespace FrontKFHShortcuts.Models.Reward
{
    public class RewardRequest
    {
        public string Title { get; set; }
        public int RequiredPoints { get; set; }
        public int Usages { get; set; }
        public DateTime DueDate { get; set; }
    }
}
