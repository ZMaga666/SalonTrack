using SalonTrackApi.Services;

namespace SalonTrackApi.Contracts

{
    public interface IServiceManager
    { 
        IExpenseService ExpenseService { get; }
        IServiceTaskService ServiceTaskService { get; }
        IServiceService ServiceService { get; }
        IIncomeService IncomeService { get; }
        IUserService UserService { get; }
    }
}
