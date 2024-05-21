namespace BackKFHShortcuts.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AwardedPoints { get; set; }
        public string Sharia {  get; set; }
        public string Image {  get; set; }
        public string TargetAudience { get; set; }
        public Category Category { get; set; }
        public List<ProductRequest> ProductRequests { get; set; }
    }
}
