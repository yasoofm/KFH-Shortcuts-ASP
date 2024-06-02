using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
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
            var dashboardResponse = await client.GetAsync("Admin/Dashboard");

            if (dashboardResponse.IsSuccessStatusCode)
            {
                var dashboardData = await dashboardResponse.Content.ReadFromJsonAsync<DashboardRequest>();

                return View(dashboardData);
            }

            return View(new DashboardRequest());
        }

    }
}
