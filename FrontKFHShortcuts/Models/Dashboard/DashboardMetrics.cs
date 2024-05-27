namespace FrontKFHShortcuts.Models.Dashboard
{
    public class DashboardMetrics
    {
        public int TotalRequests { get; set; }
        public decimal TotalCost { get; set; }
        public int ProductsSold { get; set; }
        public List<MonthlyRequest> RequestsPerMonth { get; set; }
        public List<ProductPerformance> TopProducts { get; set; }
        public List<ProductPerformance> LowestProducts { get; set; }
    }

    public class MonthlyRequest
    {
        public string Month { get; set; }
        public int Requests { get; set; }
    }

    public class ProductPerformance
    {
        public string ProductName { get; set; }
        public int Requests { get; set; }
    }
}
