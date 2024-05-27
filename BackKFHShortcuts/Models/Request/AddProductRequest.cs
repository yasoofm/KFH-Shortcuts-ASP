namespace BackKFHShortcuts.Models.Request
{
    public class AddProductRequest
    {
        public string Image {  get; set; }
        public string Shariah {  get; set; }
        public string TargetAudience { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public int AwardedPoints { get; set; }
    }
}
