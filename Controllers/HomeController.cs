using Microsoft.AspNetCore.Mvc;

namespace FiiPrezent.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}
