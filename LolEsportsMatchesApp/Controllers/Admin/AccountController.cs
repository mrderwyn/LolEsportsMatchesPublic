using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LolEsportsMatchesApp.Controllers.Admin
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthorisationService service;

        public AccountController(IAuthorisationService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await this.service.IsAdmin(model.Email, model.Password);
                if (result)
                {
                    await AuthenticateAdmin(model.Email);
                    return Redirect(model.ReturnUrl!);
                }

                ModelState.AddModelError("", "Invalid login or password.");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await this.service.CreateUser(model.Email, model.Password, model.Role);
                if (result)
                {
                    return RedirectToAction("Index", "AdminLeagues");
                }

                ModelState.AddModelError("", "Invalid login or password.");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task AuthenticateAdmin(string email)
        {
            ClaimsIdentity identity = new(
                new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"),

                },
                "Cookie");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
