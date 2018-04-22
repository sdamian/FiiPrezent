using FiiPrezent.Models;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using FiiPrezent.Db;

namespace FiiPrezent.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventsService _eventsService;
        private readonly IEventsRepository _eventsRepo;

        public HomeController(
            EventsService eventsService,
            IEventsRepository eventsRepo)
        {
            _eventsService = eventsService;
            _eventsRepo = eventsRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RsvpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Event @event = await _eventsService.RegisterParticipant(model.Code, model.Name);
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

        public async Task<IActionResult> Event(string id)
        {
            Event @event = await _eventsRepo.FindEventById(Guid.Parse(id));

            return View(new EventViewModel(@event));
        }
    }
}
