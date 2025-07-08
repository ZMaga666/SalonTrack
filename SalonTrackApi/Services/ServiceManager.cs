using Microsoft.AspNetCore.Identity;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Migrations;
using SalonTrackApi.Repositories;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public sealed class ServiceManager(IRepositoryManager repositoryManager,ILoggerManager logger, UserManager<User> userManager) : IServiceManager
    {


        private readonly Lazy<IExpenseService> _expenseService = new Lazy<IExpenseService>( () => new ExpenseService(repositoryManager,logger));
        private readonly Lazy<IServiceTaskService> _serviceTaskService = new Lazy<IServiceTaskService>( () => new ServiceTaskService(repositoryManager,logger,userManager));
        private readonly Lazy<IServiceService> _serviceService = new Lazy<IServiceService>(() => new ServiceService(repositoryManager, logger));
        private readonly Lazy<IIncomeService> _incomeService = new Lazy<IIncomeService>(() => new IncomeService(repositoryManager, logger, userManager));
        private readonly Lazy<IUserService> _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, userManager));
        public IExpenseService ExpenseService => _expenseService.Value;
        public IServiceTaskService ServiceTaskService => _serviceTaskService.Value;
        public IServiceService ServiceService => _serviceService.Value;
        public IIncomeService IncomeService => _incomeService.Value;
        public IUserService UserService => _userService.Value;
    }
}
