namespace FrontKFHShortcuts.Models.Product
{
    public class ProductRequest
    {
        public string Image { get; set; }
        public string Shariah { get; set; }
        public string TargetAudience { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int AwardedPoints { get; set; }

        //public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
