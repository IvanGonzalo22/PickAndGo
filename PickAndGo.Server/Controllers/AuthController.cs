using Microsoft.AspNetCore.Mvc;
using PickAndGo.Server.Models;
using PickAndGo.Server.Services;

namespace PickAndGo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("Invalid user data.");

            var result = await _userService.RegisterAsync(userDto);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "User registered successfully." });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var result = await _userService.LoginAsync(userDto);
            if (result.IsSuccess)
            {
                return Ok(new { Message = "Login successful", Token = result.Token });
            }
            return Unauthorized(new { Message = result.ErrorMessage });
        }
    }
}
