using SalonTrackApi.Contracts;
using SalonTrackApi.Data;
using SalonTrackApi.Repository.Contract;
using SalonTrackApi.Repository.Contracts;

namespace SalonTrackApi.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;
        private Lazy<IExpenseRepository>? _expenseRepo;
        private Lazy<IServiceTaskRepository> _serviceTaskRepo;
        private Lazy<IServiceRepository> _serviceRepo;
        private Lazy<IIncomeRepository> _incomeRepo;

        public RepositoryManager(AppDbContext context, Lazy<IServiceTaskRepository> serviceTaskRepo, Lazy<IServiceRepository> serviceRepo, Lazy<IExpenseRepository> _expenseRepo, Lazy<IIncomeRepository> _incomeRepo)

        {
            _context = context;
            _expenseRepo = new Lazy<IExpenseRepository>(() => new ExpenseRepository(_context));
            _serviceTaskRepo = serviceTaskRepo;
            _serviceRepo = serviceRepo;
            _incomeRepo = _incomeRepo;
        }

        public IExpenseRepository Expense => _expenseRepo.Value;
        public IServiceTaskRepository ServiceTask => _serviceTaskRepo.Value;

        public IServiceRepository Service => _serviceRepo.Value;

        public IIncomeRepository Income => _incomeRepo.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
          
