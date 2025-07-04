using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{
    public class IncomeRepository(AppDbContext appDbContext):RepositoryBase<Income>(appDbContext),IIncomeRepository;
    {
    }
}
