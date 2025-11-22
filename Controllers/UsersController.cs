using Asset_management.models;
using Asset_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Hosting;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;

namespace AssetManagementSystem.Controllers
{
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("User data is required.");

                if (string.IsNullOrEmpty(user.Username))
                    return BadRequest("Username is required.");

                if (string.IsNullOrEmpty(user.Email))
                    return BadRequest("Email is required.");

                if (string.IsNullOrEmpty(user.PasswordHash))
                    return BadRequest("Password is required.");

                if (string.IsNullOrEmpty(user.Role))
                    return BadRequest("Role is required.");

                var result = await _userService.RegisterUserAsync(user);

                if (!result)
                    return BadRequest("Registration failed. User may already exist.");

                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during registration: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return BadRequest("Email and password are required.");

                var user = await _userService.AuthenticateAsync(request.Email, request.Password);

                if (user == null)
                    return Unauthorized("Invalid email or password.");

                var token = _tokenService.GenerateToken(user);

                return Ok(new
                {
                    token = token,
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Login error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [Authorize]
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

        [Authorize]
        [HttpGet]
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
