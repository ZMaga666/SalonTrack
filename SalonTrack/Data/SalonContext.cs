using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalonTrack.Models;

namespace SalonTrack.Data
{
    public class SalonContext : IdentityDbContext<ApplicationUser>
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options) { }

        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Credit> Credits { get; set; }
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
