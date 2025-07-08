using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : RepositoryBase<User>(appDbContext), IUserRepository
    {
    }
}
