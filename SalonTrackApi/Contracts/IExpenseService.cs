using SalonTrackApi.Entities;


namespace SalonTrackApi.Contracts
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetAllExpenseAsync();
        Task<Expense>CreateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        
    }
}
