// Dashboard/Controllers/ServiceTaskController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonTrack.Data;
using SalonTrack.Models;
using System;
using System.Linq;

namespace SalonTrack.Contollers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class ServiceTaskController : Controller
    {
        private readonly SalonContext _context;

        public ServiceTaskController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.ServiceTasks.Include(x=>x.Income).OrderByDescending(t => t.Income.Date).ToList();

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
            task.Income.Date = DateTime.Now;
            task.Income.Username = User.Identity?.Name;
            var income = _context.Incomes.Add(task.Income).Entity;
            _context.SaveChanges();
            task.IncomeId = income.Id;
            _context.ServiceTasks.Add(task);

          

            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ServiceTasks.Find(id);
            var income = _context.Incomes.Find(task.IncomeId);
            if (task != null)
            {
                _context.ServiceTasks.Remove(task);
                _context.Incomes.Remove(income);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
