namespace FrontKFHShortcuts.Models.Reward
{
    public class RewardResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RequiredPoints { get; set; }
        public int Usages { get; set; }
        public DateTime DueDate { get; set; }
    }
}
