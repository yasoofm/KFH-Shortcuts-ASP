namespace BackKFHShortcuts.Models.Entities
{
    public class RewardRequest
    {
        public int Id { get; set; }
        public DateTime ClaimedAt { get; set; }
        public Reward Reward { get; set; }
        public User User { get; set; }
    }
}
