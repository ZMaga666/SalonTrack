using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalonTrack.Data;
using SalonTrack.Models;
using System.Linq;

namespace SalonTrack.Controllers
{
    public class ServiceController : Controller
    {
        private readonly SalonContext _context;

        public ServiceController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();

            return View();
        }


        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }
    }
}
