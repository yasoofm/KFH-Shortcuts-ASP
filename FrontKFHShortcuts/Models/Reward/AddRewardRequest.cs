namespace FrontKFHShortcuts.Models.Reward
{
    public class AddRewardRequest
    {
        public string Title { get; set; }
        public int RequieredPoints { get; set; }
        public int Usage { get; set; }
        public DateTime DueDate { get; set; }
    }
}
