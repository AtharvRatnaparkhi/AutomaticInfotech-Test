namespace TestApplication.DTO
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; } // Should be hashed in real applications
        public List<UserRoleDto> Roles { get; set; }
    }
}
