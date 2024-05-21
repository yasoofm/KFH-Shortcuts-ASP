namespace BackKFHShortcuts.Models.Entities
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientNumber { get; set; }
        DateTime CreateAt {  get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
