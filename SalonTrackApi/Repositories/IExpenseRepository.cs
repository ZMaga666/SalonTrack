using SalonTrackApi.Entities;

namespace SalonTrackApi.Repositories
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
        Task<Expense?> GetByIdAsync(int id);
        Task CreateAsync(Expense expense);
        void Delete(Expense expense);
    }
}
