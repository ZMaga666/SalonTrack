using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ServiceTask → Income cascade relation
            modelBuilder.Entity<ServiceTask>()
                .HasOne(st => st.Income)
                .WithMany()
                .HasForeignKey(st => st.IncomeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
