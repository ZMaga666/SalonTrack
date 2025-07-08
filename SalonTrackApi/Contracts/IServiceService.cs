using SalonTrackApi.Entities;

namespace SalonTrackApi.Contracts
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllServiceAsync();
        Task<Service> CreateServiceAsync(Service service);
        Task DeleteServiceAsync(int id);    
    }
}
