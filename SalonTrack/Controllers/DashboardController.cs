using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
