using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ServiceTaskController> _logger;

        public ServiceTaskController(SalonContext context, ILogger<ServiceTaskController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var tasks = _context.ServiceTasks.Include(x => x.Income)
                                             .OrderByDescending(t => t.Income.Date)
                                             .ToList();
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
            {
                _logger.LogWarning("ModelState validation failed during ServiceTask creation.");
                return View(task);
            }

            var selectedService = _context.Services.FirstOrDefault(s => s.Id == task.ServiceId);
            if (selectedService == null)
            {
                _logger.LogWarning("Selected service with ID {ServiceId} not found.", task.ServiceId);
                ModelState.AddModelError("ServiceId", "Xidmət tapılmadı.");
                return View(task);
            }

            try
            {
                task.Description = selectedService.Name;
                task.Income.Date = DateTime.Now;
                task.Income.Username = User.Identity?.Name;

                var income = _context.Incomes.Add(task.Income).Entity;
                _context.SaveChanges();

                task.IncomeId = income.Id;
                _context.ServiceTasks.Add(task);
                _context.SaveChanges();

                _logger.LogInformation("Yeni ServiceTask əlavə olundu. Task ID: {TaskId}, Username: {Username}", task.Id, task.Income.Username);

                TempData["Success"] = "Yeni iş uğurla əlavə olundu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ServiceTask yaradılarkən xəta baş verdi.");
                ModelState.AddModelError("", "Xəta baş verdi. Xahiş olunur yenidən cəhd edin.");
                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ServiceTasks.Find(id);
            var income = _context.Incomes.Find(task?.IncomeId);

            if (task != null)
            {
                _context.ServiceTasks.Remove(task);
                if (income != null)
                {
                    _context.Incomes.Remove(income);
                }
                _context.SaveChanges();
                _logger.LogInformation("ServiceTask silindi. Task ID: {TaskId}", task.Id);
            }
            else
            {
                _logger.LogWarning("Silinmək istənən ServiceTask tapılmadı. ID: {TaskId}", id);
            }

            return RedirectToAction("Index");
        }
    }
}
