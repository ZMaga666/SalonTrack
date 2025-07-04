using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contracts;

namespace SalonTrackApi.Repositories
{
    public class ServiceTaskRepository(AppDbContext appDbContext) : RepositoryBase<ServiceTask>(appDbContext),IServiceTaskRepository
    {
    }
}
