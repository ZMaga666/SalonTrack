using SalonTrackApi.Data;

namespace SalonTrackApi.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private Lazy<IExpenseRepository>? _expenseRepo;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
            _expenseRepo = new Lazy<IExpenseRepository>(() => new ExpenseRepository(_context));



        }

        public IExpenseRepository Expense => _expenseRepo.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
          
