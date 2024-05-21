using Microsoft.AspNetCore.Mvc;

namespace FrontKFHShortcuts.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
