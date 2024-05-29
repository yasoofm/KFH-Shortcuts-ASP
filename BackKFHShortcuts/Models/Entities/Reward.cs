namespace BackKFHShortcuts.Models.Entities
{
    public class Reward
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int RequiredPoints { get; set; }
        public int Usages { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<RewardRequest> Requests { get; set; }
    }
}
