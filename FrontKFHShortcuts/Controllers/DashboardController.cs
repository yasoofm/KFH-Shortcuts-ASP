using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FrontKFHShortcuts.Models.Request;

namespace FrontKFHShortcuts.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var requests = new List<RequestResponse>
            {
                new RequestResponse
                {
                    Id = 1,
                    EmployeeId = 1,
                    EmployeeName = "NAME NAME",
                    ClientNumber = "+965 XXXXXXXX",
                    ClientName = "NAME NAME",
                    ProductId = 1,
                    ProductTitle = "Organic Cream",
                    NumberOfPoints = 1234
                }
                // Add more sample data as needed
            };

            var dashboardData = new DashboardViewModel
            {
                TotalRequests = requests.Count,
                TotalCost = requests.Sum(r => r.NumberOfPoints), // Example logic, modify as needed
                ProductsSold = requests.Count * 1000, // Example logic, modify as needed
                RequestsPerMonth = GetRequestsPerMonth(requests),
                RequestSummary = GetRequestSummary(requests),
                TopProducts = GetTopProducts(requests),
                LowestProducts = GetLowestProducts(requests)
            };

            return View(dashboardData);
        }

        private List<MonthlyRequests> GetRequestsPerMonth(List<RequestResponse> requests)
        {
            // Logic to calculate requests per month
            return new List<MonthlyRequests>
            {
                new MonthlyRequests { Month = "January", Requests = 4000 },
                new MonthlyRequests { Month = "February", Requests = 3500 },
                // Add data for other months
            };
        }

        private RequestSummary GetRequestSummary(List<RequestResponse> requests)
        {
            // Logic to calculate request summary
            return new RequestSummary
            {
                Labels = new List<string> { "Product 1", "Product 2", "Product 3" }, // Example labels, modify as needed
                AverageRequests = new List<double> { 50.0, 60.0, 70.0 }, // Example data, modify as needed
                TopRequests = new List<int> { 5, 6, 7 } // Example data, modify as needed
            };
        }

        private List<Product> GetTopProducts(List<RequestResponse> requests)
        {
            // Logic to get top products
            return new List<Product>
            {
                new Product { Title = "Organic Cream", Quantity = 789 },
                new Product { Title = "Rain Umbrella", Quantity = 657 },
                new Product { Title = "Serum Bottle", Quantity = 489 }
                // Add more products as needed
            };
        }

        private List<Product> GetLowestProducts(List<RequestResponse> requests)
        {
            // Logic to get lowest products
            return new List<Product>
            {
                new Product { Title = "Serum Bottle", Quantity = 489 },
                new Product { Title = "Rain Umbrella", Quantity = 657 },
                new Product { Title = "Organic Cream", Quantity = 789 }
                // Add more products as needed
            };
        }
    }

    public class DashboardViewModel
    {
        public int TotalRequests { get; set; }
        public int TotalCost { get; set; }
        public int ProductsSold { get; set; }
        public List<MonthlyRequests> RequestsPerMonth { get; set; }
        public RequestSummary RequestSummary { get; set; }
        public List<Product> TopProducts { get; set; }
        public List<Product> LowestProducts { get; set; }
    }

    public class MonthlyRequests
    {
        public string Month { get; set; }
        public int Requests { get; set; }
    }

    public class RequestSummary
    {
        public List<string> Labels { get; set; }
        public List<double> AverageRequests { get; set; }
        public List<int> TopRequests { get; set; }
    }


    public class Product
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
    }
}
