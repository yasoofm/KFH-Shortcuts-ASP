using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FrontKFHShortcuts.Models.LogIn;
using Microsoft.AspNetCore.Http;
using FrontKFHShortcuts.Models;
using Microsoft.AspNetCore.Components;

namespace FrontKFHShortcuts.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        [CascadingParameter] private GlobalAppState MyState { get; set; }

        public LoginController(HttpClient httpClient, IConfiguration configuration, GlobalAppState state)
        {
            _configuration = configuration;
            MyState = state;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInRequest request)
        {
            if (ModelState.IsValid)
            {
                var client = MyState.createClient();
                var response = await client.PostAsJsonAsync("Authentication/Login", request);
                if(response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    MyState.SaveToken(userInfo);
                    return RedirectToAction("Index", "Dashboard");
                }

                
            }

            return View("index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            MyState.RemoveToken();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Welcome()
        {
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            return View();
        }
    }
}