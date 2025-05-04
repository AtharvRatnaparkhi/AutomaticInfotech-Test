using TestApplication.DTO;

namespace TestApplication.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(UserDto user);
        Task<bool> UpdateUserAsync(UserDto user);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> ValidateUserCredentials(string username, string password);

        // User + Role specific
        Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(int userId);
    }
}
