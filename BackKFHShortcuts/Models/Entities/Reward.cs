namespace BackKFHShortcuts.Models.Entities
{
    public class Reward
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RequiredPoints { get; set; }
        public List<RewardRequest> Requests { get; set; }
    }
}
