using SalonTrackApi.Contracts;
using SalonTrackApi.Data;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private IExpenseRepository _expenseRepo;
        private IServiceTaskRepository _serviceTaskRepo;
        private IServiceRepository _serviceRepo;
        private IIncomeRepository _incomeRepo;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
        }

        public IExpenseRepository Expense =>
            _expenseRepo ??= new ExpenseRepository(_context);

        public IServiceTaskRepository ServiceTask =>
            _serviceTaskRepo ??= new ServiceTaskRepository(_context);

        public IServiceRepository Service =>
            _serviceRepo ??= new ServiceRepository(_context);

        public IIncomeRepository Income =>
            _incomeRepo ??= new IncomeRepository(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}