using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.DTOs;
using REDZAuthApi.Services;
using System.Text.Json;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnbanController : ControllerBase
    {
        private readonly BlacklistService _blacklistService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UnbanController(BlacklistService blacklistService, HttpClient httpClient, IConfiguration configuration)
        {
            _blacklistService = blacklistService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Unban([FromBody] UnbanDTO request)
        {
            if (!request.UnbanUser && !request.UnbanIP && !request.UnbanHWID)
                return BadRequest("Select at least one option to unban.");

            if (request.UnbanUser && string.IsNullOrEmpty(request.Username))
                return BadRequest("Username is required to unban user.");

            if (request.UnbanIP && string.IsNullOrEmpty(request.IP))
                return BadRequest("IP is required to unban IP.");

            if (request.UnbanHWID && string.IsNullOrEmpty(request.HWID))
                return BadRequest("HWID is required to unban HWID.");

            await _blacklistService.UnbanAsync(
                request.UnbanUser ? request.Username : null,
                request.UnbanIP ? request.IP : null,
                request.UnbanHWID ? request.HWID : null
            );

            await SendWebhookUnban(request);

            return Ok("Unban completed successfully.");
        }

        private async Task SendWebhookUnban(UnbanDTO req)
        {
            try
            {
                var webhookUrl = _configuration["WebhookSettings:DiscordWebhookUrl"];
                if (string.IsNullOrEmpty(webhookUrl) || webhookUrl == "YOUR_DISCORD_WEBHOOK_URL_HERE")
                    return; // Skip webhook if not configured

                var embed = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = "✅ Unban Completed",
                            color = 0x00FF00,
                            fields = new[]
                            {
                                new { name = "User", value = req.Username ?? "N/A", inline = true },
                                new { name = "IP", value = req.IP ?? "N/A", inline = true },
                                new { name = "HWID", value = req.HWID ?? "N/A", inline = true },
                                new { name = "Unban User?", value = req.UnbanUser.ToString(), inline = true },
                                new { name = "Unban IP?", value = req.UnbanIP.ToString(), inline = true },
                                new { name = "Unban HWID?", value = req.UnbanHWID.ToString(), inline = true },
                                new { name = "Reason", value = req.Reason ?? "Reason not provided", inline = false },
                                new { name = "Date", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                            }
                        }
                    }
                };

                var json = new StringContent(JsonSerializer.Serialize(embed), System.Text.Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(webhookUrl, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook error: {ex.Message}");
            }
        }
    }
}
