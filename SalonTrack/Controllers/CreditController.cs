// Dashboard/Controllers/CreditController.cs
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
    public class CreditController : Controller
    {
        private readonly SalonContext _context;

        public CreditController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var credits = _context.Credits.OrderByDescending(c => c.Date).ToList();
            return View(credits);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Credit credit)
        {
            if (ModelState.IsValid)
            {
                credit.Date = DateTime.Now;
                _context.Credits.Add(credit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credit);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var credit = _context.Credits.Find(id);
            if (credit != null)
            {
                _context.Credits.Remove(credit);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MarkAsPaid(int id)
        {
            var credit = _context.Credits.Find(id);
            if (credit != null)
            {
                credit.IsPaid = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
