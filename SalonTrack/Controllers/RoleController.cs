using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalonTrack.Models;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        ViewBag.Roles = new SelectList(roles); // ✅ DropDown üçün göndərilir
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(string username, string password, string role)
    {
        if (await _userManager.FindByNameAsync(username) != null)
            return BadRequest("İstifadəçi artıq mövcuddur.");

        var user = new ApplicationUser { UserName = username };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }

    [HttpGet]
    public IActionResult CreateRole() => View();

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        return RedirectToAction("CreateUser");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }

        return RedirectToAction("Index");
    }

}
