namespace SalonTrackApi.Services
{
    public interface IServiceManager
    { 
        IExpenseService ExpenseService { get; }
    }
}
