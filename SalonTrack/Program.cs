using Microsoft.EntityFrameworkCore;
using SalonTrack.Data;

namespace SalonTrack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SalonContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddAuthentication("SalonCookie")
    .AddCookie("SalonCookie", options =>
    {
        options.LoginPath = "/Account/Login";
<<<<<<< HEAD
        options.AccessDeniedPath = "/Account/Logout";
    });
            builder.Services.AddAuthorization();
            builder.Services.AddSession();
=======
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
>>>>>>> 628c14e2a8aca7943f06cdbe48c7f06a240d03be
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
<<<<<<< HEAD
            app.UseSession();
=======

>>>>>>> 628c14e2a8aca7943f06cdbe48c7f06a240d03be
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
<<<<<<< HEAD
            app.UseAuthentication();
            app.UseAuthorization();
            //app.MapAreaControllerRoute(
            //       name: "Areas",
            //       areaName: "dashboard",
            //       pattern: "admin/{controller=Account}/{action=Login}/{id?}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

=======

            app.UseAuthorization();
            app.MapControllerRoute(
    name: "dashboard",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
>>>>>>> 628c14e2a8aca7943f06cdbe48c7f06a240d03be

            app.Run();
        }
    }
}
