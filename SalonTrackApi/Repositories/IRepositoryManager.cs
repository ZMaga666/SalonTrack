namespace SalonTrackApi.Repositories
{
    public interface IRepositoryManager
    {
        IExpenseRepository Expense { get; }
        Task SaveAsync();
    }
}
