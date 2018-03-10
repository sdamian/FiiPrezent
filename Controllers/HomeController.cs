using FiiPrezent.Models;
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

        [HttpPost]
        public IActionResult Index(RsvpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}
