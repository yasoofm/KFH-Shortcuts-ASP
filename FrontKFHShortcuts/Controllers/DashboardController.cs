using Microsoft.AspNetCore.Mvc;

namespace FrontKFHShortcuts.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
