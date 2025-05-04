using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TestApplication.DTO;
using TestApplication.Interfaces;

namespace TestApplication.Services
{
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public RoleService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<RoleDto>("sp_GetAllRoles", commandType: CommandType.StoredProcedure);
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<RoleDto>(
                "sp_GetRoleById", new { ID = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateRoleAsync(RoleDto role)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "sp_CreateRole", new { role.RoleName, role.RoleCode }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateRoleAsync(RoleDto role)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "sp_UpdateRole", new { role.RoleID, role.RoleName, role.RoleCode }, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync("sp_DeleteRole", new { ID = id }, commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
