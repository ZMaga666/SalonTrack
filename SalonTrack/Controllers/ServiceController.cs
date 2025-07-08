using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalonTrack.Data;
using SalonTrack.Models;
using System.Linq;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly SalonContext _context;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(SalonContext context, ILogger<ServiceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
            _logger.LogInformation("ServiceController.Servisler cagirildi");
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