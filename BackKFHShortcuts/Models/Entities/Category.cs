namespace BackKFHShortcuts.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Product> Products { get; set; }
    }
}
