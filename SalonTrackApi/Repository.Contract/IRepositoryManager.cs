using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repository.Contract
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
