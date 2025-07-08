using SalonTrackApi.Entities;
using SalonTrackApi.Results;

namespace SalonTrackApi.Contracts
{
    public interface IIncomeService
    {
        Task<PaginatedIncomeResult> GetFilteredIncomesAsync(string? userId, DateTime? startDate, DateTime? endDate, bool showDeactivated, int page);
        Task<Income?> GetByIdAsync(int id);
        Task<Income> UpdateIncomeAsync(int id, Income updated);
        Task DeleteIncomeAsync(int id);
    }
}
