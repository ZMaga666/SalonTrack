using SalonTrackApi.LoggerService;
using SalonTrackApi.Migrations;
using SalonTrackApi.Repositories;

namespace SalonTrackApi.Services
{
    public sealed class ServiceManager(IRepositoryManager repositoryManager,ILoggerManager logger) : IServiceManager
    {


        private readonly Lazy<IExpenseService> _expenseService = new Lazy<IExpenseService>( () => new ExpenseService(repositoryManager,logger));
        public IExpenseService ExpenseService => _expenseService.Value;
    }
}
