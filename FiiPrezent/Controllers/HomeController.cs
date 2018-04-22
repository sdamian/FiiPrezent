using FiiPrezent.Models;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FiiPrezent.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FiiPrezent.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventsService _eventsService;
        private readonly IEventsRepository _eventsRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            EventsService eventsService,
            IEventsRepository eventsRepo, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
        {
            _eventsService = eventsService;
            _eventsRepo = eventsRepo;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string fullName = String.Empty;
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                fullName = user.FullName;
            }

            return View(new RsvpViewModel
            {
                Name = fullName
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(RsvpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Event @event;
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                @event = await _eventsService.RegisterParticipant(model.Code, user.FullName, user.PhotoUrl);
            }
            else
            {
                string photoUrl = Url.Content("~/missing-picture.png");
                @event = await _eventsService.RegisterParticipant(model.Code, model.Name, photoUrl);
            }
            
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
