using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.DTOs;
using REDZAuthApi.Models;
using REDZAuthApi.Services;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(IMongoDatabase database)
        {
            _authService = new AuthService(database);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Key))
                return BadRequest(new { message = "Username, password and key are required." });

            (User? user, string? error) = await _authService.RegisterAsync(dto, HttpContext);

            if (user == null)
                return BadRequest(new { message = error });

            return Ok(new { message = "Account Created Successfully", username = user.Username, plan = user.Plan, expiration = user.Expiration });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
                return BadRequest(new { message = "Username and password are required." });

            (User? user, string? error) = await _authService.LoginAsync(dto, HttpContext);

            if (user == null)
                return BadRequest(new { message = error });

            return Ok(new { message = "Login Successful", username = user.Username, plan = user.Plan, expiration = user.Expiration });
        }
    }
}
