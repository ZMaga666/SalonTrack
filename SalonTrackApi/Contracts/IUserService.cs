using SalonTrackApi.DTO;
using SalonTrackApi.Entities;

namespace SalonTrackApi.Contracts
{
    public interface IUserService
    {
        Task<List<UserListDto>> GetFilteredUsersAsync(DateTime? startDate, DateTime? endDate, string status);
        Task<UserEditDto?> GetUserByIdAsync(string id);
        Task<User> CreateUserAsync(UserCreateDto dto);
        Task UpdateUserAsync(UserEditDto dto);
        Task ActivateUserAsync(string id);
        Task SoftDeleteUserAsync(string id);

    }
}
