using SalonTrackApi.Repository.Contract;
using SalonTrackApi.Repository.Contracts;

namespace SalonTrackApi.Repositories
{
    public interface IRepositoryManager
    {
        IExpenseRepository Expense { get; }
        IServiceTaskRepository ServiceTask { get; }
        IServiceRepository Service { get; }
        IIncomeRepository Income { get; }
        Task SaveAsync();
    }
}
