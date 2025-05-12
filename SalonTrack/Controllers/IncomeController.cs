using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.ViewModels;
using System.Linq;

namespace SalonTrack.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        private readonly SalonContext _context;

        public IncomeController(SalonContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var incomes = _context.Incomes.ToList();
            var expenses = _context.Expenses.ToList(); // ✅ xərclər də əlavə olundu

            var now = DateTime.Now;
            var today = now.Date;
            var thisWeekStart = now.AddDays(-(int)now.DayOfWeek);
            var thisMonthStart = new DateTime(now.Year, now.Month, 1);
            var thisYearStart = new DateTime(now.Year, 1, 1);

            var model = new IncomeListViewModel
            {
                Incomes = incomes.OrderByDescending(i => i.Date).ToList(),
                Total = incomes.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount), // ✅ burda xərclər toplanır

                TodayTotal = incomes.Where(i => i.Date.Date == today).Sum(i => i.Amount),
                ThisWeekTotal = incomes.Where(i => i.Date.Date >= thisWeekStart.Date).Sum(i => i.Amount),
                ThisMonthTotal = incomes.Where(i => i.Date.Date >= thisMonthStart.Date).Sum(i => i.Amount),
                ThisYearTotal = incomes.Where(i => i.Date.Date >= thisYearStart.Date).Sum(i => i.Amount)
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
            var model = new IncomeListViewModel
            {
                Incomes = incomes.OrderByDescending(i => i.Date).ToList(),
                Total = total,
                StartDate = startDate,
                EndDate = endDate
            };

            return View("Index", model); // eyni View istifadə olunur
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
