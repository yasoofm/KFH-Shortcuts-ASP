namespace BackKFHShortcuts.Models.Request
{
    public class EditRewardRequest
    {
        public string? Title { get; set; }
        public int? RequiredPoints { get; set; }
        public int? Usages { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
