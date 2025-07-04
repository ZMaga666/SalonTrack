using SalonTrackApi.Entities;

namespace SalonTrackApi.Contracts
{
    public interface IServiceTaskService
    {

        Task<IEnumerable<ServiceTask>> GetTodayTasksAsync();
        Task<ServiceTask> CreateServiceTaskAsync(ServiceTask task, string? overrideUserId = null);
        Task DeleteServiceTaskAsync(int id);
    }
}
