using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Interfaces;

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto user)
        {
            var result = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = result }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto user)
        {
            if (id != user.UserID)
                return BadRequest();

            var updated = await _userService.UpdateUserAsync(user);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetRolesByUserId(int userId)
        {
            var roles = await _userService.GetRolesByUserIdAsync(userId);
            return Ok(roles);
        }
    }
}
