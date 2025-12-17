using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AnalizorWebApp.Models;

namespace AnalizorWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("/auth/login")]
        public IActionResult Login() => View();

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username, password, true, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Dashboard");

            ViewBag.Error = "Hatalı kullanıcı adı veya şifre";
            return View();
        }

        [HttpGet("/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet("/auth/denied")]
        public IActionResult Denied() => View();
    }
}
