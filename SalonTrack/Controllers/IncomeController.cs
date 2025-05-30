using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.ViewModels;
using System;
using System.Linq;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncomeController : Controller
    {
        private readonly SalonContext _context;
        private readonly ILogger<IncomeController> _logger;

        public IncomeController(SalonContext context, ILogger<IncomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(string? username)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            _logger.LogInformation("IncomeController.Index çağırıldı. Filter: {Username}", username);

            var incomes = _context.Incomes.AsQueryable();
            if (!string.IsNullOrEmpty(username))
            {
                incomes = incomes.Where(i => i.Username == username);
            }

            var expenses = _context.Expenses.ToList();
            var now = DateTime.Now;
            var today = now.Date;
            var thisWeekStart = now.AddDays(-(int)now.DayOfWeek);
            var thisMonthStart = new DateTime(now.Year, now.Month, 1);
            var thisYearStart = new DateTime(now.Year, 1, 1);

            var incomeList = incomes.ToList();

            var model = new IncomeListViewModel
            {
                Incomes = incomeList.OrderByDescending(i => i.Date).ToList(),
                Total = incomeList.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount),

                TodayTotal = incomeList.Where(i => i.Date.Date == today).Sum(i => i.Amount),
                ThisWeekTotal = incomeList.Where(i => i.Date >= thisWeekStart).Sum(i => i.Amount),
                ThisMonthTotal = incomeList.Where(i => i.Date >= thisMonthStart).Sum(i => i.Amount),
                ThisYearTotal = incomeList.Where(i => i.Date >= thisYearStart).Sum(i => i.Amount),

                SelectedUsername = username,
                AllUsernames = _context.Incomes.Select(i => i.Username).Distinct().ToList()
            };

            return View(model);
        }

        public IActionResult FilteredList(DateTime? startDate, DateTime? endDate, string? username)
        {
            _logger.LogInformation("IncomeController.FilteredList çağırıldı. Tarixlər: {Start} - {End}, İstifadəçi: {Username}", startDate, endDate, username);

            var incomes = _context.Incomes.AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                incomes = incomes.Where(i => i.Username == username);
            }

            if (startDate.HasValue)
            {
                incomes = incomes.Where(i => i.Date.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                incomes = incomes.Where(i => i.Date.Date <= endDate.Value.Date);
            }

            var incomeList = incomes.ToList();
            var total = incomeList.Sum(i => i.Amount);

            var model = new IncomeListViewModel
            {
                Incomes = incomeList.OrderByDescending(i => i.Date).ToList(),
                Total = total,
                StartDate = startDate,
                EndDate = endDate,
                SelectedUsername = username,
                AllUsernames = _context.Incomes.Select(i => i.Username).Distinct().ToList()
            };

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
            {
                _logger.LogWarning("IncomeController.Edit GET - gəlir tapılmadı. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("IncomeController.Edit GET - ID: {Id}", id);
            return View(income);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Income updated)
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
