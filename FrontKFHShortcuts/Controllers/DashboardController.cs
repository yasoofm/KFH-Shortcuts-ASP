using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Dashboard;
using FrontKFHShortcuts.Models.Product;
using FrontKFHShortcuts.Models.Request;
using FrontKFHShortcuts.Models.Reward;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FrontKFHShortcuts.Controllers
{
    public class DashboardController : Controller
    {
        private readonly GlobalAppState MyState;

        public DashboardController(GlobalAppState state)
        {
            MyState = state;
        }

        public async Task<IActionResult> Index()
        {
            var client = MyState.createClient();
            var productResponse = await client.GetAsync("Admin/GetProduct");
            var requestResponse = await client.GetAsync("Admin/GetProductRequest");
            var rewardResponse = await client.GetAsync("Admin/GetReward");

            if (productResponse.IsSuccessStatusCode && requestResponse.IsSuccessStatusCode && rewardResponse.IsSuccessStatusCode)
            {
                var products = await productResponse.Content.ReadFromJsonAsync<List<ProductResponse>>();
                var requests = await requestResponse.Content.ReadFromJsonAsync<List<RequestResponse>>();
                var rewards = await rewardResponse.Content.ReadFromJsonAsync<List<RewardResponse>>();

                var dashboardMetrics = new DashboardMetrics
                {
                    TotalRequests = requests.Count,
                    TotalCost = rewards.Sum(r => r.RequiredPoints), // Assume points represent cost
                    ProductsSold = products.Sum(p => p.Requests), // Assume requests represent sales
                    RequestsPerMonth = requests.GroupBy(r => r.RequestDate.Month)
                                               .Select(g => new MonthlyRequest
                                               {
                                                   Month = new DateTime(1, g.Key, 1).ToString("MMM"),
                                                   Requests = g.Count()
                                               }).ToList(),
                    TopProducts = products.OrderByDescending(p => p.Requests)
                                          .Take(5)
                                          .Select(p => new ProductPerformance
                                          {
                                              ProductName = p.Name,
                                              Requests = p.Requests
                                          }).ToList(),
                    LowestProducts = products.OrderBy(p => p.Requests)
                                             .Take(5)
                                             .Select(p => new ProductPerformance
                                             {
                                                 ProductName = p.Name,
                                                 Requests = p.Requests
                                             }).ToList()
                };

                return View(dashboardMetrics);
            }

            return View(new DashboardMetrics());
        }
    }
}
