using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonTrackApi.Contracts;
using SalonTrackApi.DTO;
using SalonTrackApi.Entities;
using SalonTrackApi.LoggerService;
using SalonTrackApi.Repository.Contract;

namespace SalonTrackApi.Services
{
    public class UserService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IRepositoryManager repository,
    ILoggerManager logger,
    IMapper mapper) : IUserService
    {
        public async Task<List<UserListDto>> GetFilteredUsersAsync(DateTime? startDate, DateTime? endDate, string status)
        {
            var users = await userManager.Users.ToListAsync();

            users = status switch
            {
                "all" => users,
                "active" => users.Where(u => !u.IsDeleted).ToList(),
                "deactive" => users.Where(u => u.IsDeleted).ToList(),
                _ => users
            };

            var result = new List<UserListDto>();

            foreach (var user in users)
            {
                var dto = mapper.Map<UserListDto>(user);
                var role = await userManager.GetRolesAsync(user);
                var totalIncome = repository.Income.FindByCondition(i =>
                        i.UserId == user.Id &&
                        (!startDate.HasValue || i.Date >= startDate) &&
                        (!endDate.HasValue || i.Date <= endDate),
                    false).Sum(i => (decimal?)i.Amount) ?? 0;

                dto.Role = role.FirstOrDefault() ?? "-";
                dto.TotalIncome = totalIncome;

                result.Add(dto);
            }

            return result;
        }

        public async Task<User> CreateUserAsync(UserCreateDto dto)
        {
          //  if (await userManager.FindByNameAsync(dto.UserName) is not null)
           //     throw new Exception("Bu istifadəçi adı artıq mövcuddur.");

            var user = new User
            {
                UserName = dto.UserName,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

          //  await userManager.AddToRoleAsync(user, dto.Role);
            return user;
        }

        public async Task<UserEditDto?> GetUserByIdAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) return null;

            var dto = mapper.Map<UserEditDto>(user);
            var roles = await userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? "-";

            return dto;
        }

        public async Task UpdateUserAsync(UserEditDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.Id) ?? throw new Exception("İstifadəçi tapılmadı.");

            user.UserName = dto.UserName;
            user.IsDeleted = dto.IsDeleted;

            var existingRoles = await userManager.GetRolesAsync(user);
            if (existingRoles.Any())
                await userManager.RemoveFromRolesAsync(user, existingRoles);

            await userManager.AddToRoleAsync(user, dto.Role);
            await userManager.UpdateAsync(user);
        }

        public async Task ActivateUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id) ?? throw new Exception("İstifadəçi tapılmadı.");
            user.IsDeleted = false;
            await userManager.UpdateAsync(user);
        }

        public async Task SoftDeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id) ?? throw new Exception("İstifadəçi tapılmadı.");
            user.IsDeleted = true;
            await userManager.UpdateAsync(user);
        }
    }

}
