using MASH.Models;
using MASH.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MASH.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseDAL _databaseDAL;

        public AccountController(DatabaseDAL databaseDAL)
        {
            _databaseDAL = databaseDAL;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            var user = await _databaseDAL.GetUser(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(
                model.Password,
                user.PasswordHash);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        // method to register a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Register(Registration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _databaseDAL.GetUser(model.Username)!= null)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }
            if (await _databaseDAL.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            await _databaseDAL.AddUser(model.Username, model.Email, passwordHash);
            return RedirectToAction("Login");
        }
    }
}