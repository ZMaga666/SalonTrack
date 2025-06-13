using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalonTrack.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SalonContext _context;

    public UserController(UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          SalonContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string status = "active")
    {
        var users = await _userManager.Users.ToListAsync();

        users = status switch
        {
            "all" => users,
            "active"=> users.Where(u=>!u.IsDeleted).ToList(),
            "deactive"=> users.Where(u => u.IsDeleted).ToList()
        };

        var viewModel = new List<UserListViewModel>();

        foreach (var user in users)
        {
            var role = await _userManager.GetRolesAsync(user);
            var totalIncome = _context.Incomes
                .Where(i => i.UserId == user.Id &&
                            (!startDate.HasValue || i.Date >= startDate) &&
                            (!endDate.HasValue || i.Date <= endDate))
                .Sum(i => (decimal?)i.Amount) ?? 0;

            viewModel.Add(new UserListViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = role.FirstOrDefault() ?? "-",
                CreatedAt = user.CreatedAt,
                TotalIncome = totalIncome,
                IsDeleted = user.IsDeleted
            });
        }

        // ViewBag ilə filter üçün lazım olan məlumatları ötür
        ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
        ViewBag.Status = status;

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Activate(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["Error"] = "İstifadəçi tapılmadı.";
            return RedirectToAction("Index");
        }

        user.IsDeleted = false;
        await _userManager.UpdateAsync(user);

        TempData["Success"] = "İstifadəçi aktiv edildi.";
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string username, string password, string role)
    {
        if (await _userManager.FindByNameAsync(username) != null)
        {
            TempData["Error"] = "Bu istifadəçi adı artıq mövcuddur.";
            return RedirectToAction("Create");
        }

        var user = new ApplicationUser
        {
            UserName = username,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
            TempData["Success"] = "İstifadəçi uğurla yaradıldı.";
            return RedirectToAction("Index");
        }

        TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        return RedirectToAction("Create");


    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        var model = new UserEditViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = roles.FirstOrDefault() ?? "",
            IsDeleted = user.IsDeleted
        };

        ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", model.Role);
        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name", model.Role);
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
            return NotFound();

        user.UserName = model.UserName;
        user.IsDeleted = model.IsDeleted;

        var existingRoles = await _userManager.GetRolesAsync(user);
        if (existingRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
        }
        await _userManager.AddToRoleAsync(user, model.Role);

        await _userManager.UpdateAsync(user);

        TempData["Success"] = "İstifadəçi uğurla yeniləndi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["Error"] = "İstifadəçi tapılmadı.";
            return RedirectToAction("Index");
        }

        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);

        TempData["Success"] = "İstifadəçi silindi ";
        return RedirectToAction("Index");
    }

}