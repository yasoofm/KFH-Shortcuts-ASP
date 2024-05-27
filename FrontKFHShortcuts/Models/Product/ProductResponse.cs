namespace FrontKFHShortcuts.Models.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Shariah { get; set; }
        public string TargetAudience { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AwardedPoints { get; set; }
        public string CategoryName { get; set; }
        public int Requests { get; set; }  // New property to track requests
    }
}
