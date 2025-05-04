using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TestApplication.DTO;
using TestApplication.Interfaces;

namespace TestApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public UserService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<UserDto> ValidateUserCredentials(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);

            // Call a stored procedure or direct query to validate the user
            var user = await connection.QueryFirstOrDefaultAsync<UserDto>(
                "sp_ValidateUserCredentials", new { UserName = username, Password = password }, commandType: CommandType.StoredProcedure);

            if (user != null)
            {
                // Fetch roles associated with the user
                var roles = await connection.QueryAsync<UserRoleDto>(
                    "sp_GetRolesByUserId", new { UserID = user.UserID }, commandType: CommandType.StoredProcedure);

                // Attach roles to the user
                user.Roles = roles.ToList();
            }

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<UserDto>("sp_GetAllUsers", commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<UserDto>(
                "sp_GetUserById", new { UserId = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateUserAsync(UserDto user)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(
                "sp_CreateUser", new { user.UserName, user.Password }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "sp_UpdateUser", new { user.UserID, user.UserName, user.Password }, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.ExecuteAsync("sp_DeleteUser", new { ID = id }, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<IEnumerable<UserRoleDto>> GetRolesByUserIdAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<UserRoleDto>(
                "sp_GetRolesByUserId", new { UserID = userId }, commandType: CommandType.StoredProcedure);
        }
    }
}
