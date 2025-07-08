using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public class ServiceService(IRepositoryManager repository, ILoggerManager logger) : IServiceService
    {
        public async Task<Service> CreateServiceAsync(Service service)
        {
             repository.Service.Create(service);
            await repository.SaveAsync();
            logger.LogInfo($"Service with ID {service.Id} created successfully.");
            return service;
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = repository.Service.FindByCondition(s => s.Id == id, trackChanges: false).FirstOrDefault();
            if (service == null)
            {
                logger.LogError($"Service with ID {id} not found.");
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            repository.Service.Delete(service);
            await repository.SaveAsync();
            logger.LogInfo($"Service with ID {id} deleted successfully.");
        }

        public async Task<IEnumerable<Service>> GetAllServiceAsync()
        {
            var services = repository.Service.FindAll(false).ToList();
                
                logger.LogInfo("Fetching all services.");
            return services;
        }
    }
}
