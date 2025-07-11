using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Contracts;
using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repositories;
using SalonTrackApi.Repository.Contract;
using SalonTrackApi.Services;

namespace SalonTrackApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
           services.AddIdentity<User, IdentityRole>(options =>
            {
                // Configure Identity options here if needed
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
 .AddEntityFrameworkStores<AppDbContext>()
 .AddDefaultTokenProviders();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
           

          

           
        }
    }
}

