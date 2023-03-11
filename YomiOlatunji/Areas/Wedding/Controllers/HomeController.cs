using Microsoft.AspNetCore.Mvc;

namespace YomiOlatunji.Areas.Wedding.Controllers
{
    [Area("Wedding")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
