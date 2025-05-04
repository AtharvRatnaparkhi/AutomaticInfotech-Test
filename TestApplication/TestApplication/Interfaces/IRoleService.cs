using TestApplication.DTO;

namespace TestApplication.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(RoleDto role);
        Task<bool> UpdateRoleAsync(RoleDto role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
