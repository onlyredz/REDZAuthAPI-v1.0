using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.Services;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/hwid")]
    public class HWIDController : ControllerBase
    {
        private readonly AuthService _authService;

        public HWIDController(IMongoDatabase database)
        {
            _authService = new AuthService(database);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetHWID([FromBody] HWIDResetDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
                return BadRequest("Username é obrigatório.");

            var (success, message) = await _authService.ResetHWIDAsync(dto.Username);

            if (!success)
                return BadRequest(message);

            return Ok(message);
        }
    }

    public class HWIDResetDTO
    {
        public string Username { get; set; } = string.Empty;
    }
}
