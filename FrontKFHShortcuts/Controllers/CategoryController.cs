using Microsoft.AspNetCore.Mvc;

namespace FrontKFHShortcuts.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
