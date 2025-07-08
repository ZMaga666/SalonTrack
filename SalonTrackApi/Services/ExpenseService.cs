using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repositories;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public class ExpenseService(IRepositoryManager repositoryManager, ILoggerManager logger) : IExpenseService
    {
        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            expense.Date = DateTime.Now;
            repositoryManager.Expense.Create(expense);
            await repositoryManager.SaveAsync();
            logger.LogInfo($"Expense created with ID: {expense.Id}");
            return expense;
        }

        public Task DeleteExpenseAsync(int id)
        {
           var expense = repositoryManager.Expense.FindByCondition(e => e.Id == id, false).FirstOrDefault();
            if (expense != null)
            {
                repositoryManager.Expense.Delete(expense);
                return repositoryManager.SaveAsync();
            }
            logger.LogError($"Expense with ID: {id} not found for deletion.");
            throw new KeyNotFoundException($"Expense with ID: {id} not found.");
        }

        public async  Task<IEnumerable<Expense>> GetAllExpenseAsync()
        {
            var expense =  repositoryManager.Expense.FindAll(false);
            logger.LogInfo("GetAllExpenseAsync called");
            return expense;

        }
        public async Task<Expense?> GetByIdAsync(int id)
        {
            return repositoryManager.Expense.FindByCondition(e => e.Id == id, false).FirstOrDefault();
            logger.LogInfo($"Expense called for ID :{id}");
        }

    }
}

