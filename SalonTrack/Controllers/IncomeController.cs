// Controller
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
        private const int PageSize = 10;

        public IncomeController(SalonContext context, ILogger<IncomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? userId, DateTime? startDate, DateTime? endDate,bool showDeactivated = false, int page = 1)
        {
            var incomes = _context.Incomes.Include(i => i.User).AsQueryable();

            var usersQuery = _userManager.Users.AsQueryable();
            if (!showDeactivated)
            {
                usersQuery = usersQuery.Where(u => !u.IsDeleted);
            }
            var allUsers = await usersQuery.ToListAsync();

            if (!string.IsNullOrEmpty(userId))
                incomes = incomes.Where(i => i.UserId == userId);

            if (startDate.HasValue)
                incomes = incomes.Where(i => i.Date >= startDate);

            if (endDate.HasValue)
                incomes = incomes.Where(i => i.Date <= endDate);

            var totalCount = await incomes.CountAsync();

            var pagedIncomes = await incomes
                .OrderByDescending(i => i.Date)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewData["Page"] = page;

            var expenses = await _context.Expenses.ToListAsync();
            var now = DateTime.Now;
            var today = now.Date;
            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var model = new IncomeListViewModel
            {
                Incomes = pagedIncomes,
                Total = incomes.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount),
               // NetTotal = incomes.Sum(i => i.Amount) - expenses.Sum(e => e.Amount),
                TodayTotal = incomes.Where(i => i.Date.Date == today).Sum(i => i.Amount),
                ThisWeekTotal = incomes.Where(i => i.Date >= weekStart).Sum(i => i.Amount),
                ThisMonthTotal = incomes.Where(i => i.Date >= monthStart).Sum(i => i.Amount),
                ThisYearTotal = incomes.Where(i => i.Date >= yearStart).Sum(i => i.Amount),
                SelectedUserId = userId,
                AllUsers = allUsers,
                StartDate = startDate,
                EndDate = endDate,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
                ShowDeactivated = showDeactivated
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_IncomeTablePartial", model);
            }

            return View(model);
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
