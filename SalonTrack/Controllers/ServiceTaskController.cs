// Dashboard/Controllers/ServiceTaskController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SalonTrack.Data;
using SalonTrack.Models;
using System;
using System.Linq;

namespace SalonTrack.Contollers
{
    [Authorize]
    public class ServiceTaskController : Controller
    {
        private readonly SalonContext _context;

        public ServiceTaskController(SalonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.ServiceTasks.OrderByDescending(t => t.Date).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ServiceTask task)
        {
            if (ModelState.IsValid)
            {
                task.Date = DateTime.Now;
                _context.ServiceTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ServiceTasks.Find(id);
            if (task != null)
            {
                _context.ServiceTasks.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
