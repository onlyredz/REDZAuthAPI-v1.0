using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.Services;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AuthService _authService;

        public AdminController(IMongoDatabase database)
        {
            _authService = new AuthService(database);
        }

        [HttpPost("reset-hwid")]
        public async Task<IActionResult> ResetHWID([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest(new { message = "Username is required." });

            var (success, msg) = await _authService.ResetHWIDAsync(username);
            if (!success)
                return BadRequest(new { message = msg });

            return Ok(new { message = msg });
        }
    }
}
