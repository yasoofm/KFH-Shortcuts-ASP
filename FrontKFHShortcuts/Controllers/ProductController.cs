using Microsoft.AspNetCore.Mvc;

namespace FrontKFHShortcuts.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
