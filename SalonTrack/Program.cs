using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonTrack.Data;
using SalonTrack.Models;
using SalonTrack.Helpers;
using NLog;
using NLog.Web;

namespace SalonTrack
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Application starting...");

                var builder = WebApplication.CreateBuilder(args);

                // NLog konfiqurasiya
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                builder.Host.UseNLog(); // NLog-u Host-a əlavə et

                // Verilənlər bazası
                builder.Services.AddDbContext<SalonContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                // Identity
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<SalonContext>()
                    .AddDefaultTokenProviders();

                // Cookie-based auth
                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.Name = "SalonCookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 🔐 HTTPS cookie
                    options.Cookie.SameSite = SameSiteMode.Strict;
                });

                builder.Services.AddAuthorization();
                builder.Services.AddSession();
                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                // Seed default roles/admin
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await SeedData.SeedRolesAndAdminAsync(services);
                }

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();
                app.UseSession();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Program stopped because of exception.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
