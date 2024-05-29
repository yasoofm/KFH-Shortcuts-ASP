namespace BackKFHShortcuts.Models.Responses
{
    public class DashboardResponse
    {
        public int TotalRequests { get; set; }
        public List<RequestPerMonth> MonthlyRequests {  get; set; }
        public List<RequestPerProduct> ProductRequests { get; set; }
        public List<RequestPerProduct> TopProducts { get; set; }
        public List<RequestPerProduct> LeastProducts { get; set; }
    }

    public class RequestPerMonth
    {
        public string Month { get; set; }
        public int Requests { get; set; }
    }

    public class RequestPerProduct
    {
        public string Image { get; set; }
        public string ProductName { get; set;}
        public int Requests { get; set; }
    }
}
