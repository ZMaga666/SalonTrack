using Microsoft.AspNetCore.Identity;
using SalonTrackApi.Contracts;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repositories;
using SalonTrackApi.Entities;
using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public class ServiceTaskService(
    IRepositoryManager repository,
    ILoggerManager logger,
    UserManager<User> userManager
) : IServiceTaskService
    {
        public async Task<IEnumerable<ServiceTask>> GetTodayTasksAsync()
        {
            var today = DateTime.Today;

            var tasks = await repository.ServiceTask
                .FindByCondition(t => t.Date.Date == today, trackChanges: false)
                .Include(t => t.Income)
                    .ThenInclude(i => i.User)
                .Include(t => t.Service)
                .Include(t => t.User)
                .OrderByDescending(t => t.Date)
                .Take(12)
                .ToListAsync();

            return tasks;
        }

        public async Task<ServiceTask> CreateServiceTaskAsync(ServiceTask task, string? overrideUserId = null)
        {
            var selectedService = await repository.Service.FindByCondition(s => s.Id == task.ServiceId, false).FirstOrDefaultAsync();
            if (selectedService == null)
                throw new Exception("Seçilmiş xidmət mövcud deyil.");

            var userId = overrideUserId ?? task.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("İstifadəçi təyin edilməyib.");

            task.Description = selectedService.Name;
            task.Date = DateTime.Now;
            task.UserId = userId;

            var income = new Income
            {
                Amount = task.Price,
                Date = task.Date,
                UserId = userId
            };

            repository.Income.Create(income);
            await repository.SaveAsync();

            task.IncomeId = income.Id;
            repository.ServiceTask.Create(task);
            await repository.SaveAsync();

            logger.LogInfo($"Yeni ServiceTask əlavə edildi: ID={task.Id}");
            return task;
        }

        public async Task DeleteServiceTaskAsync(int id)
        {
            var task = await repository.ServiceTask.FindByCondition(t => t.Id == id, true).FirstOrDefaultAsync();
            if (task == null)
            {
                logger.LogWarn($"Silinəcək ServiceTask tapılmadı: ID={id}");
                throw new Exception("ServiceTask tapılmadı.");
            }

            var income = await repository.Income.FindByCondition(i => i.Id == task.IncomeId, true).FirstOrDefaultAsync();

            repository.ServiceTask.Delete(task);
            if (income != null)
                repository.Income.Delete(income);

            await repository.SaveAsync();
            logger.LogInfo($"ServiceTask və bağlı income silindi: ID={id}");
        }
    }


}
