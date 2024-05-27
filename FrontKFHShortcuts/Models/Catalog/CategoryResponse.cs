namespace FrontKFHShortcuts.Models.Catalog
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Requests { get; set; }  // New property to track requests
    }
}
