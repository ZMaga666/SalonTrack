// Dashboard/Controllers/ServiceTaskController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SalonTrack.Data;
using SalonTrack.Models;
using System;
using System.Linq;

namespace SalonTrack.Contollers
{
    [Authorize]
    public class ServiceTaskController : Controller
    {
        private readonly SalonContext _context;

        public ServiceTaskController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.ServiceTasks.OrderByDescending(t => t.Date).ToList();
            return View(tasks);
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
        public IActionResult Create(ServiceTask task)
        {
            ViewBag.Services = _context.Services
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();

            if (!ModelState.IsValid)
                return View(task);

            var selectedService = _context.Services.FirstOrDefault(s => s.Id == task.ServiceId);
            if (selectedService == null)
            {
                ModelState.AddModelError("ServiceId", "Xidmət tapılmadı.");
                return View(task);
            }

            
            task.Description = selectedService.Name;
            task.Date = DateTime.Now;

            _context.ServiceTasks.Add(task);

            if (!task.IsCredit)
            {
                var income = new Income
                {
                    Amount = task.Price,
                    Date = task.Date
                };
                _context.Incomes.Add(income);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ServiceTasks.Find(id);
            if (task != null)
            {
                _context.ServiceTasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
