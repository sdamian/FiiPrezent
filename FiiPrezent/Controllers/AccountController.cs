using FiiPrezent.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FiiPrezent.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            _logger.LogInformation("Challenging {provier} with {props}", provider, properties);

            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(String.Empty, remoteError);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogWarning("Could not get external login info...");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (!result.Succeeded)
            {
                await RegisterNewUserAndSignIn(info);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task RegisterNewUserAndSignIn(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var profilePicture = $"https://graph.facebook.com/{identifier}/picture?type=large";

            var user = new ApplicationUser
            {
                UserName = email,
                FullName = name,
                Email = email,
                PhotoUrl = profilePicture
            };
            var userResult = await _userManager.CreateAsync(user);
            if (userResult.Succeeded)
            {
                userResult = await _userManager.AddLoginAsync(user, info);
                if (userResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
