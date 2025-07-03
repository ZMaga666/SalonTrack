using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Data;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Repositories
{

    public class ExpenseRepository(AppDbContext appDbContext) : RepositoryBase<Expense>(appDbContext), IExpenseRepository
    {
        
    }

}
