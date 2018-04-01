using System;
using FiiPrezent.Models.Admin;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Mvc;

namespace FiiPrezent.Controllers
{
    public class AdminController : Controller
    {
        private readonly EventsService _eventsService;

        public AdminController(EventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // GET
        public IActionResult Index()
        {
            var result = new[]
            {
                new EventListItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Event 1",
                    Description = "Event description",
                    SecretCode = "secret"
                },
                new EventListItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Event 2",
                    Description = "Event description",
                    SecretCode = "secret"
                }
            };
            return View(result);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Create Event";
            return View("Edit");
        }

        public IActionResult Update()
        {
            ViewBag.Title = "Update Event";
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}
