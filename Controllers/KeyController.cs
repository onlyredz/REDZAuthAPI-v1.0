using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.Models;
using REDZAuthApi.Services;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeyController : ControllerBase
    {
        private readonly LicenseService _licenseService;

        public KeyController(LicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] string plan)
        {
            if (string.IsNullOrEmpty(plan))
                return BadRequest(new { message = "Plan is required (mensal, trimestral, anual, lifetime)" });

            var allowedPlans = new[] { "mensal", "trimestral", "anual", "lifetime" };
            if (!allowedPlans.Contains(plan.ToLower()))
                return BadRequest(new { message = "Invalid plan. Allowed: mensal, trimestral, anual, lifetime" });

            var license = await _licenseService.CreateLicenseAsync(plan.ToLower());
            return Ok(new { message = "Key generated successfully", key = license.Key });
        }

        [HttpPost("custom")]
        public async Task<IActionResult> Custom([FromQuery] string plan, [FromQuery] string customKey)
        {
            if (string.IsNullOrEmpty(plan) || string.IsNullOrEmpty(customKey))
                return BadRequest(new { message = "Both plan and customKey are required" });

            var allowedPlans = new[] { "mensal", "trimestral", "anual", "lifetime" };
            if (!allowedPlans.Contains(plan.ToLower()))
                return BadRequest(new { message = "Invalid plan. Allowed: mensal, trimestral, anual, lifetime" });

            if (await _licenseService.KeyExistsAsync(customKey))
                return BadRequest(new { message = "This key already exists. Choose another one." });

            var (success, error) = await _licenseService.CreateCustomLicenseAsync(customKey, plan.ToLower());

            if (!success)
                return StatusCode(500, new { message = error ?? "Failed to create custom key." });

            return Ok(new { message = "Custom key created successfully", key = customKey });
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListKeys(
            [FromQuery] string? plan,
            [FromQuery] bool? used,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var filter = Builders<License>.Filter.Empty;

            if (!string.IsNullOrEmpty(plan))
                filter &= Builders<License>.Filter.Eq(l => l.Plan, plan.ToLower());

            if (used.HasValue)
            {
                if (used.Value)
                    filter &= Builders<License>.Filter.Ne(l => l.UsedBy, null);
                else
                    filter &= Builders<License>.Filter.Eq(l => l.UsedBy, null);
            }

            if (fromDate.HasValue)
                filter &= Builders<License>.Filter.Gte(l => l.CreatedAt, fromDate.Value);

            if (toDate.HasValue)
                filter &= Builders<License>.Filter.Lte(l => l.CreatedAt, toDate.Value);

            var keys = await _licenseService.GetFilteredKeysAsync(filter);

            var result = keys.Select(k => new
            {
                k.Key,
                k.Plan,
                k.UsedBy,
                k.Expiration,
                k.CreatedAt
            });

            return Ok(result);
        }
    }
}
