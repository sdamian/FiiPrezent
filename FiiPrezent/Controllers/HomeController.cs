using FiiPrezent.Models;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace FiiPrezent.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventsService _eventsService;

        public HomeController(EventsService eventsService)
        {
            _eventsService = eventsService;
        }

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

            Event @event = _eventsService.RegisterParticipant(model.Code, model.Name);
            if (@event == null)
            {
                ModelState.AddModelError<RsvpViewModel>(x => x.Code, "Wrong verification code");
                return View(model);
            }

            return RedirectToAction("Event", new
            {
                @event.Id
            });
        }

        public IActionResult Event(string id)
        {
            Event @event = _eventsService.FindEventById(Guid.Parse(id));

            return View(new EventViewModel(@event));
        }
    }
}
