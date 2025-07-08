using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Contracts;
using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repository.Contract;
using SalonTrackApi.Results;

namespace SalonTrackApi.Services
{
    public class IncomeService(
    IRepositoryManager repository,
    ILoggerManager logger,
    UserManager<User> userManager
) : IIncomeService
    {
        private const int PageSize = 10;

        public async Task<PaginatedIncomeResult> GetFilteredIncomesAsync(string? userId, DateTime? startDate, DateTime? endDate, bool showDeactivated, int page)
        {
            var incomes = repository.Income.FindAll(false, "User");

            if (!string.IsNullOrEmpty(userId))
                incomes = incomes.Where(i => i.UserId == userId);

            if (startDate.HasValue)
                incomes = incomes.Where(i => i.Date >= startDate);

            if (endDate.HasValue)
                incomes = incomes.Where(i => i.Date <= endDate);

            var totalCount = await incomes.CountAsync();

            var pagedIncomes = await incomes
                .OrderByDescending(i => i.Date)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var allUsers = userManager.Users.AsQueryable();
            if (!showDeactivated)
                allUsers = allUsers.Where(u => !u.IsDeleted);

            var users = await allUsers.ToListAsync();
            var expenses = await repository.Expense.FindAll(false).ToListAsync();
            var now = DateTime.Now;
            var today = now.Date;
            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            return new PaginatedIncomeResult
            {
                Incomes = pagedIncomes,
                Total = incomes.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount),
                TodayTotal = incomes.Where(i => i.Date.Date == today).Sum(i => i.Amount),
                ThisWeekTotal = incomes.Where(i => i.Date >= weekStart).Sum(i => i.Amount),
                ThisMonthTotal = incomes.Where(i => i.Date >= monthStart).Sum(i => i.Amount),
                ThisYearTotal = incomes.Where(i => i.Date >= yearStart).Sum(i => i.Amount),
                AllUsers = users,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize)
            };
        }

        public async Task<Income?> GetByIdAsync(int id)
        {
            return await repository.Income.FindByCondition(i => i.Id == id, false).FirstOrDefaultAsync();
        }

        public async Task<Income> UpdateIncomeAsync(int id, Income updated)
        {
            var income = await repository.Income.FindByCondition(i => i.Id == id, true).FirstOrDefaultAsync();
            if (income is null) throw new Exception("Gəlir tapılmadı.");

            income.Amount = updated.Amount;
            income.Date = updated.Date;
            income.Username = updated.Username;

            if (!string.IsNullOrEmpty(updated.Username))
            {
                var user = await userManager.FindByNameAsync(updated.Username);
                if (user != null)
                    income.UserId = user.Id;
            }

            await repository.SaveAsync();
            return income;
        }

        public async Task DeleteIncomeAsync(int id)
        {
            var income = await repository.Income.FindByCondition(i => i.Id == id, true).FirstOrDefaultAsync();
            if (income is null) throw new Exception("Gəlir tapılmadı.");

            repository.Income.Delete(income);
            await repository.SaveAsync();
        }
    }

}
