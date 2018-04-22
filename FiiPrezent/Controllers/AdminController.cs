using System;
using System.Linq;
using System.Threading.Tasks;
using FiiPrezent.Db;
using FiiPrezent.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiiPrezent.Controllers
{
    public class AdminController : Controller
    {
        private readonly EventsDbContext _db;

        public AdminController(EventsDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            EventListItem[] eventListItems = await _db.Events
                .Select(x => new EventListItem
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    SecretCode = x.VerificationCode,
                })
                .ToArrayAsync();

            return View(eventListItems);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Create Event";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEvent model)
        {
            _db.Events.Add(new Event
            {
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            });
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid id)
        {
            ViewBag.Title = "Update Event";
            CreateEvent model = await _db.Events
                .Select(x => new CreateEvent
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Description = x.Description,
                    SecretCode = x.VerificationCode,
                })
                .SingleAsync(x => x.Id == id.ToString());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CreateEvent model)
        {
            Event @event = new Event
            {
                Id = new Guid(model.Id),
                Name = model.Name,
                Description = model.Description,
                VerificationCode = model.SecretCode
            };

            _db.Events.Update(@event);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            _db.Remove(new Event
            {
                Id = id
            });
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
