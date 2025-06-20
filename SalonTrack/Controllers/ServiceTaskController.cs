using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalonTrack.Data;
using SalonTrack.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Moderator,Admin")]
    public class ServiceTaskController : Controller
    {
        private readonly SalonContext _context;
        private readonly ILogger<ServiceTaskController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceTaskController(SalonContext context, ILogger<ServiceTaskController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var tasks = _context.ServiceTasks
                                .Include(t => t.Income)
                                    .ThenInclude(i => i.User)
                                .Include(t => t.Service)
                                .Include(t => t.User)
                                .Where(t=>t.Date.Date == DateTime.Today)
                                .OrderByDescending(t => t.Date)
                                .Take(12)
                                .ToList();

            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            if (User.IsInRole("Admin"))
            {
                ViewBag.Users = _userManager.Users
                    .Where(u => !u.IsDeleted)
                    .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
                    .ToList();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceTask task)
        {
            ViewBag.Services = _context.Services
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            // if (User.IsInRole("Admin"))
            //  {
            ViewBag.Users = _userManager.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
                .ToList();
            //  }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState validation failed during ServiceTask creation.");

                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;

                    foreach (var error in errors)
                    {
                        _logger.LogError("ModelState Error - Property: {Key}, Error: {Error}", key, error.ErrorMessage);
                    }
                }

                return View(task);
            }


            var selectedService = await _context.Services.FindAsync(task.ServiceId);
            if (selectedService == null)
            {
                ModelState.AddModelError("ServiceId", "Seçilmiş xidmət mövcud deyil.");
                return View(task);
            }

            var userId = User.IsInRole("Admin") && !string.IsNullOrEmpty(task.UserId)
                         ? task.UserId
                         : _userManager.GetUserId(User);

            try
            {
                task.Description = selectedService.Name;
                task.Date = DateTime.Now;
                task.UserId = userId;

                var income = new Income
                {
                    Amount = task.Price,
                    Date = task.Date,
                    UserId = userId
                };

                _context.Incomes.Add(income);
                await _context.SaveChangesAsync();

                task.IncomeId = income.Id;
                _context.ServiceTasks.Add(task);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Yeni ServiceTask əlavə olundu. Task ID: {TaskId}", task.Id);
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
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ServiceTasks.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("ServiceTask tapılmadı. ID: {TaskId}", id);
                return RedirectToAction("Index");
            }

            var income = await _context.Incomes.FindAsync(task.IncomeId);

            _context.ServiceTasks.Remove(task);
            if (income != null)
            {
                _context.Incomes.Remove(income);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("ServiceTask silindi. Task ID: {TaskId}", task.Id);
            TempData["Success"] = "Iş uğurla silindi.";
            return RedirectToAction("Index");
        }
    }
}
