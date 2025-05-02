using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SalonTrack.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Threading.Tasks;


namespace SalonTrack.Controllers
{
 


    public class AccountController : Controller
    {
        private readonly SalonContext _context;

        public AccountController(SalonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "İstifadəçi adı və ya parol yalnışdır.";
                return View();
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

            var identity = new ClaimsIdentity(claims, "SalonCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("SalonCookie", principal);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("SalonCookie");
            return RedirectToAction("Login");
        }
    }

}
