using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Data;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Repositories
{
    
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;
        public ExpenseRepository(AppDbContext context) => _context = context;

        public async Task<List<Expense>> GetAllAsync() =>
            await _context.Expenses.OrderByDescending(e => e.Date).ToListAsync();

        public async Task<Expense?> GetByIdAsync(int id) =>
            await _context.Expenses.FindAsync(id);

        public async Task CreateAsync(Expense expense) =>
            await _context.Expenses.AddAsync(expense);

        public void Delete(Expense expense) =>
            _context.Expenses.Remove(expense);
    }

}
