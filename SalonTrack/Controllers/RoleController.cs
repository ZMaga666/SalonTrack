using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonTrack.Data;
using SalonTrack.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SalonContext _context;

    public RoleController(RoleManager<IdentityRole> roleManager,
                          UserManager<ApplicationUser> userManager,
                          SalonContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        ViewBag.Roles = new SelectList(roles);

        var roleList = _roleManager.Roles.AsNoTracking().ToList();
        return View(roleList);
    }

    

    [HttpGet]
    public IActionResult CreateRole() => View();

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            TempData["Success"] = result.Succeeded
                ? $"'{roleName}' rolu yaradıldı."
                : "Rol yaradılarkən xəta baş verdi.";
        }
        else
        {
            TempData["Warning"] = $"'{roleName}' rolu artıq mövcuddur.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            TempData["Error"] = "Rol tapılmadı.";
            return RedirectToAction("Index");
        }

        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

        foreach (var user in usersInRole)
        {
            
            user.IsDeleted = true;
            user.LockoutEnd = DateTime.MaxValue;
            await _userManager.UpdateAsync(user);
        }

        await _roleManager.DeleteAsync(role);
        TempData["Success"] = $"'{roleName}' rolu və əlaqəli istifadəçilər bloklandı.";
        return RedirectToAction("Index");
    }
}
