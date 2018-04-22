using System;
using System.Linq;
using FiiPrezent.Db;
using FiiPrezent.Models.Admin;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Mvc;

namespace FiiPrezent.Controllers
{
    public class AdminController : Controller
    {
        private readonly EventsDbContext _db;
        private readonly IEventsRepository _eventsRepository;

        public AdminController(EventsDbContext db, IEventsRepository eventsRepository)
        {
            _db = db;
            _eventsRepository = eventsRepository;
        }

        // GET
        public IActionResult Index()
        {
            return View(_db.Events.ToList().Select(x => new EventListItem(x)).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Create Event";
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEvent model)
        {
            Event @event = new Event
            {
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            };

            _eventsRepository.Add(@event);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.Title = "Update Event";

            return View(new CreateEvent(_eventsRepository.FindEventById(id)));
        }

        [HttpPost]
        public IActionResult Update(CreateEvent model)
        {
            Event @event = new Event
            {
                Id = new Guid(model.Id),
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            };

            _eventsRepository.Update(@event);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _eventsRepository.Delete(id);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
