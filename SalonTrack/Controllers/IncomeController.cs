using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncomeController : Controller
    {
        private readonly SalonContext _context;
        private readonly ILogger<IncomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncomeController(SalonContext context, ILogger<IncomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index(string? userId)
        {
            var incomes = _context.Incomes.Include(i => i.User).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                incomes = incomes.Where(i => i.UserId == userId);
            }

            var expenses = _context.Expenses.ToList();
            var now = DateTime.Now;
            var today = now.Date;
            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var incomeList = incomes.ToList();

            var model = new IncomeListViewModel
            {
                Incomes = incomeList.OrderByDescending(i => i.Date).ToList(),
                Total = incomeList.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount),

                TodayTotal = incomeList.Where(i => i.Date.Date == today).Sum(i => i.Amount),
                ThisWeekTotal = incomeList.Where(i => i.Date >= weekStart).Sum(i => i.Amount),
                ThisMonthTotal = incomeList.Where(i => i.Date >= monthStart).Sum(i => i.Amount),
                ThisYearTotal = incomeList.Where(i => i.Date >= yearStart).Sum(i => i.Amount),

                SelectedUserId = userId,
                AllUsers = _userManager.Users.Where(u => !u.IsDeleted).ToList()
            };

            return View(model);
        }
       
        public IActionResult FilteredList(DateTime? startDate, DateTime? endDate, string? userId)
        {
            var incomes = _context.Incomes.Include(i => i.User).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                incomes = incomes.Where(i => i.UserId == userId);
            }

            if (startDate.HasValue)
                incomes = incomes.Where(i => i.Date >= startDate);

            if (endDate.HasValue)
                incomes = incomes.Where(i => i.Date <= endDate);

            var incomeList = incomes.ToList();
            var total = incomeList.Sum(i => i.Amount);

            var model = new IncomeListViewModel
            {
                Incomes = incomeList.OrderByDescending(i => i.Date).ToList(),
                Total = total,
                StartDate = startDate,
                EndDate = endDate,
                SelectedUserId = userId,
                AllUsers = _userManager.Users.Where(u => !u.IsDeleted).ToList()
            };

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var income = _context.Incomes.Include(i => i.User).FirstOrDefault(i => i.Id == id);
            if (income == null)
            {
                _logger.LogWarning("IncomeController.Edit GET - gəlir tapılmadı. ID: {Id}", id);
                return NotFound();
            }

            ViewBag.UserList = _userManager.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new SelectListItem
                {
                    Value = u.UserName,
                    Text = u.UserName
                }).ToList();

            return View(income);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Income updated)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("IncomeController.Edit POST - model valid deyil. ID: {Id}", id);
                return View(updated);
            }

            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
            {
                _logger.LogWarning("IncomeController.Edit POST - gəlir tapılmadı. ID: {Id}", id);
                return NotFound();
            }

            income.Amount = updated.Amount;
            income.Date = updated.Date;
            income.Username = updated.Username;

            if (!string.IsNullOrEmpty(updated.Username))
            {
                var user = await _userManager.FindByNameAsync(updated.Username);
                if (user != null)
                {
                    income.UserId = user.Id;
                }
            }

            _context.SaveChanges();

            _logger.LogInformation("IncomeController.Edit - gəlir yeniləndi. ID: {Id}", id);
            TempData["Success"] = "Gəlir uğurla yeniləndi.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
            {
                _logger.LogWarning("IncomeController.Delete - gəlir tapılmadı. ID: {Id}", id);
                return NotFound();
            }

            _context.Incomes.Remove(income);
            _context.SaveChanges();

            _logger.LogInformation("IncomeController.Delete - gəlir silindi. ID: {Id}", id);
            return RedirectToAction("Index");
        }
    }
}
