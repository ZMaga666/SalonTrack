using SalonTrackApi.Data;

namespace SalonTrackApi.Repositories
{
    public class RepositoryManager
    {
        private readonly AppDbContext _context;
        private IExpenseRepository? _expense;

        public RepositoryManager(AppDbContext context) => _context = context;

        public IExpenseRepository Expense => _expense ??= new ExpenseRepository(_context);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
