using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FrontKFHShortcuts.Controllers
{
    public class RequestController : Controller
    {
        private readonly GlobalAppState MyState;

        public RequestController(GlobalAppState state)
        {
            MyState = state;
        }

        // GET: Request
        public async Task<IActionResult> Index()
        {
            var client = MyState.createClient();
            var response = await client.GetAsync("Admin/GetProductRequest");
            if (response.IsSuccessStatusCode)
            {
                var requests = await response.Content.ReadFromJsonAsync<List<RequestResponse>>();
                return View(requests);
            }
            return View(null);
        }
    }
}
