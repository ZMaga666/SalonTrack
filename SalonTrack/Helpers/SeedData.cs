using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SalonTrack.Models;

namespace SalonTrack.Helpers
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Moderator" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            string adminUsername = "admin";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = "admin@salon.local",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
