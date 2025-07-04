using SalonTrackApi.Services;

namespace SalonTrackApi.Contracts

{
    public interface IServiceManager
    { 
        IExpenseService ExpenseService { get; }
        IServiceTaskService ServiceTaskService { get; }
    }
}
