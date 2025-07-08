using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{
    public class ServiceTaskRepository(AppDbContext appDbContext) : RepositoryBase<ServiceTask>(appDbContext),IServiceTaskRepository
    {
    }
}
