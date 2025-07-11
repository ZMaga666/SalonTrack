using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SalonTrackApi.Contracts;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repositories;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        private readonly Lazy<IExpenseService> _expenseService;
        private readonly Lazy<IServiceTaskService> _serviceTaskService;
        private readonly Lazy<IServiceService> _serviceService;
        private readonly Lazy<IIncomeService> _incomeService;
        private readonly Lazy<IUserService> _userService;

        public ServiceManager(
            IRepositoryManager repositoryManager,
            ILoggerManager logger,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;

            _expenseService = new Lazy<IExpenseService>(() =>
                new ExpenseService(_repositoryManager, _logger));

            _serviceTaskService = new Lazy<IServiceTaskService>(() =>
                new ServiceTaskService(_repositoryManager, _logger, _userManager));

            _serviceService = new Lazy<IServiceService>(() =>
                new ServiceService(_repositoryManager, _logger));

            _incomeService = new Lazy<IIncomeService>(() =>
                new IncomeService(_repositoryManager, _logger, _userManager));

            _userService = new Lazy<IUserService>(() =>
                new UserService(_userManager, _roleManager, _repositoryManager, _logger, _mapper));
        }

        public IExpenseService ExpenseService => _expenseService.Value;
        public IServiceTaskService ServiceTaskService => _serviceTaskService.Value;
        public IServiceService ServiceService => _serviceService.Value;
        public IIncomeService IncomeService => _incomeService.Value;
        public IUserService UserService => _userService.Value;
    }
}