// Dashboard/Controllers/ExpenseController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonTrack;
using SalonTrack.Data;
using SalonTrack.Models;
using System;
using System.Linq;

namespace SalonTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly SalonContext _context;

        public ExpenseController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses.OrderByDescending(e => e.Date).ToList();
            return View(expenses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.Date = DateTime.Now;
                _context.Expenses.Add(expense);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
