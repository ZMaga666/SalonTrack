using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{
    public class ServiceRepository(AppDbContext appDbContext) : RepositoryBase<Service>(appDbContext), IServiceRepository
    {
    }
}
