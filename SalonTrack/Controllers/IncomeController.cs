using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.ViewModels;
using System.Linq;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncomeController : Controller
    {
        private readonly SalonContext _context;

        public IncomeController(SalonContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? username)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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



        public IActionResult FilteredList(DateTime? startDate, DateTime? endDate)
        {
            var incomes = _context.Incomes.AsQueryable();

            if (startDate.HasValue || endDate.HasValue)
            {
                if (startDate.HasValue && endDate.HasValue)
                {
                    incomes = incomes.Where(i => i.Date.Date >= startDate.Value.Date && i.Date.Date <= endDate.Value.Date);
                }
                else if (startDate.HasValue)
                {
                    incomes = incomes.Where(i => i.Date.Date >= startDate.Value.Date);
                }
                else if (endDate.HasValue)
                {
                    incomes = incomes.Where(i => i.Date.Date <= endDate.Value.Date);
                }
            }

            var total = incomes.Sum(i => i.Amount);
            var incomeList = incomes.ToList();

            var model = new IncomeListViewModel
            {
                Incomes = incomeList.OrderByDescending(i => i.Date).ToList(),
                Total = total,
                StartDate = startDate,
                EndDate = endDate,

                // ⬇️ Əlavə et:
                SelectedUsername = null, // bu filterdə yoxdur
                AllUsernames = _context.Incomes.Select(i => i.Username).Distinct().ToList()
            };

            return View("Index", model);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Income income)
        //{
        //    if (!ModelState.IsValid)
        //        return View(income);

        //    income.Date = income.Date.ToLocalTime();
        //    _context.Incomes.Add(income);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
                return NotFound();

            return View(income);
        }

        [HttpPost]
        public IActionResult Edit(int id, Income updated)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
                return NotFound();

            income.Amount = updated.Amount;
            income.Date = updated.Date;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var income = _context.Incomes.FirstOrDefault(i => i.Id == id);
            if (income == null)
                return NotFound();

            _context.Incomes.Remove(income);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
