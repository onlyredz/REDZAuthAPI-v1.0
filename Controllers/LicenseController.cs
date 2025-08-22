using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.Services;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly LicenseService _licenseService;

        public LicenseController(LicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] string plan)
        {
            if (string.IsNullOrEmpty(plan))
                return BadRequest(new { message = "Plan is required (mensal, trimestral, anual, lifetime)" });

            var license = await _licenseService.CreateLicenseAsync(plan);
            return Ok(new
            {
                message = "License Key Created Successfully",
                key = license.Key,
                plan = license.Plan,
                expiration = license.Expiration.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        [HttpPost("custom")]
        public async Task<IActionResult> CreateCustom([FromQuery] string customKey, [FromQuery] string plan)
        {
            if (string.IsNullOrEmpty(customKey) || string.IsNullOrEmpty(plan))
                return BadRequest(new { message = "customKey and plan are required." });

            var (success, error) = await _licenseService.CreateCustomLicenseAsync(customKey, plan);

            if (!success)
                return BadRequest(new { message = error });

            return Ok(new { message = "Custom License Key Created Successfully", key = customKey, plan = plan });
        }
    }
}
