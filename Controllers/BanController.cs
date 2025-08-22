using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REDZAuthApi.Services;
using System.Text.Json;

namespace REDZAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanController : ControllerBase
    {
        private readonly BlacklistService _blacklistService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BanController(BlacklistService blacklistService, HttpClient httpClient, IConfiguration configuration)
        {
            _blacklistService = blacklistService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Ban([FromBody] BanRequest request)
        {
            if (!request.BanUser && !request.BanIP && !request.BanHWID)
                return BadRequest("Select at least one ban option.");

            if (request.BanUser && string.IsNullOrEmpty(request.Username))
                return BadRequest("Username is required to ban user.");

            if (request.BanIP && string.IsNullOrEmpty(request.IP))
                return BadRequest("IP is required to ban IP.");

            if (request.BanHWID && string.IsNullOrEmpty(request.HWID))
                return BadRequest("HWID is required to ban HWID.");

            await _blacklistService.BanAsync(
                request.BanUser ? request.Username : null,
                request.BanIP ? request.IP : null,
                request.BanHWID ? request.HWID : null,
                request.Reason
            );

            await SendWebhookBan(request);

            return Ok("Ban registered successfully.");
        }

        private async Task SendWebhookBan(BanRequest req)
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
                            title = "🔨 User Banned",
                            color = 0xFF0000,
                            fields = new[]
                            {
                                new { name = "User", value = req.Username ?? "N/A", inline = true },
                                new { name = "IP", value = req.IP ?? "N/A", inline = true },
                                new { name = "HWID", value = req.HWID ?? "N/A", inline = true },
                                new { name = "Ban User?", value = req.BanUser.ToString(), inline = true },
                                new { name = "Ban IP?", value = req.BanIP.ToString(), inline = true },
                                new { name = "Ban HWID?", value = req.BanHWID.ToString(), inline = true },
                                new { name = "Reason", value = req.Reason ?? "N/A", inline = false },
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

    public class BanRequest
    {
        public string? Username { get; set; }
        public string? IP { get; set; }
        public string? HWID { get; set; }
        public bool BanUser { get; set; }
        public bool BanIP { get; set; }
        public bool BanHWID { get; set; }
        public string Reason { get; set; } = "Reason not provided";
    }
}
