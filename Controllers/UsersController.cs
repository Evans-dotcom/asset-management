using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;

        public UsersController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(user);
                if (!result)
                    return BadRequest("Registration failed. User may already exist.");

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during registration: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("Invalid email or password.");

                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Login error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            try
            {
                var email = User.Identity?.Name;
                var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                return Ok(new { email, role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to fetch user info: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet]
        // [Authorize(Roles = "admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving users: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
