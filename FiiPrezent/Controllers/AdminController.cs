using System;
using System.Linq;
using FiiPrezent.Db;
using FiiPrezent.Models.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FiiPrezent.Controllers
{
    public class AdminController : Controller
    {
        private readonly EventsDbContext _db;

        public AdminController(EventsDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            EventListItem[] eventListItems = _db.Events
                .Select(x => new EventListItem
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    SecretCode = x.VerificationCode,
                })
                .ToArray();

            return View(eventListItems);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Create Event";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEvent model)
        {
            _db.Events.Add(new Event
            {
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            });
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.Title = "Update Event";
            CreateEvent model = _db.Events
                .Select(x => new CreateEvent
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    SecretCode = x.VerificationCode,
                })
                .Single(x => x.Id == id.ToString());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CreateEvent model)
        {
            Event @event = new Event
            {
                Id = new Guid(model.Id),
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            };

            _db.Events.Update(@event);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            _db.Remove(new Event
            {
                Id = id
            });
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
