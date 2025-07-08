using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Data;
using SalonTrackApi.Entities;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Repositories
{

    public class ExpenseRepository(AppDbContext appDbContext) : RepositoryBase<Expense>(appDbContext), IExpenseRepository
    {
        
    }

}
