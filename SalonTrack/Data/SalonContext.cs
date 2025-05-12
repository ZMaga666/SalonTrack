using Microsoft.EntityFrameworkCore;
using SalonTrack.Models;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace SalonTrack.Data
{

    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Seed an initial user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123" // NOT SECURE – just for demo
            });
        }
    }
}
