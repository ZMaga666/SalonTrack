using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Contracts;
using SalonTrackApi.Data;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Services;

namespace SalonTrackApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IExpenseService, ExpenseService>();
        }
    }
}

