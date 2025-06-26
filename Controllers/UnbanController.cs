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

        public UnbanController(IMongoDatabase database)
        {
            _blacklistService = new BlacklistService(database);
        }

        [HttpPost]
        public async Task<IActionResult> Unban([FromBody] UnbanDTO request)
        {
            if (!request.UnbanUser && !request.UnbanIP && !request.UnbanHWID)
                return BadRequest("Selecione pelo menos uma opção para desbanir.");

            if (request.UnbanUser && string.IsNullOrEmpty(request.Username))
                return BadRequest("Informe o username para desbanir.");

            if (request.UnbanIP && string.IsNullOrEmpty(request.IP))
                return BadRequest("Informe o IP para desbanir.");

            if (request.UnbanHWID && string.IsNullOrEmpty(request.HWID))
                return BadRequest("Informe o HWID para desbanir.");

            await _blacklistService.UnbanAsync(
                request.UnbanUser ? request.Username : null,
                request.UnbanIP ? request.IP : null,
                request.UnbanHWID ? request.HWID : null
            );

            await SendWebhookUnban(request);

            return Ok("Desbanimento realizado com sucesso.");
        }

        private async Task SendWebhookUnban(UnbanDTO req)
        {
            var embed = new
            {
                embeds = new[]
                {
                    new
                    {
                        title = "✅ Desbanimento realizado",
                        color = 0x00FF00,
                        fields = new[]
                        {
                            new { name = "Usuário", value = req.Username ?? "N/A", inline = true },
                            new { name = "IP", value = req.IP ?? "N/A", inline = true },
                            new { name = "HWID", value = req.HWID ?? "N/A", inline = true },
                            new { name = "Desbanir usuário?", value = req.UnbanUser.ToString(), inline = true },
                            new { name = "Desbanir IP?", value = req.UnbanIP.ToString(), inline = true },
                            new { name = "Desbanir HWID?", value = req.UnbanHWID.ToString(), inline = true },
                            new { name = "Motivo", value = req.Reason ?? "Motivo não informado", inline = false },
                            new { name = "Data", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };

            using var http = new HttpClient();
            var json = new StringContent(JsonSerializer.Serialize(embed), System.Text.Encoding.UTF8, "application/json");

            await http.PostAsync("https://discord.com/api/webhooks/1322751086775500872/eHM8IgEIyLFxBqgurJNlXyaGzHi4rNjWr16XtmjWsAra7yI12u5vQcNuzlMLM6nQ0aH7", json);
        }
    }
}
