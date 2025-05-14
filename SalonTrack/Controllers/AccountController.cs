﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalonTrack.Models;

namespace SalonTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                ViewBag.Error = "İstifadəçi tapılmadı.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ViewBag.Error = "Parol yalnışdır.";
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Rol əsaslı yönləndirmə
            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "Dashboard");
            else if (roles.Contains("Moderator"))
                return RedirectToAction("Index", "ServiceTask");

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return Content("Bu səhifəyə giriş icazəniz yoxdur.");
        }
    }
}
