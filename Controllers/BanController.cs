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

        public BanController(IMongoDatabase database)
        {
            _blacklistService = new BlacklistService(database);
        }

        [HttpPost]
        public async Task<IActionResult> Ban([FromBody] BanRequest request)
        {
            if (!request.BanUser && !request.BanIP && !request.BanHWID)
                return BadRequest("Selecione pelo menos uma opção de banimento.");

            if (request.BanUser && string.IsNullOrEmpty(request.Username))
                return BadRequest("Informe o username para banir.");

            if (request.BanIP && string.IsNullOrEmpty(request.IP))
                return BadRequest("Informe o IP para banir.");

            if (request.BanHWID && string.IsNullOrEmpty(request.HWID))
                return BadRequest("Informe o HWID para banir.");

            await _blacklistService.BanAsync(
                request.BanUser ? request.Username : null,
                request.BanIP ? request.IP : null,
                request.BanHWID ? request.HWID : null,
                request.Reason
            );

            await SendWebhookBan(request);

            return Ok("Banimento registrado com sucesso.");
        }

        private async Task SendWebhookBan(BanRequest req)
        {
            var embed = new
            {
                embeds = new[]
                {
                    new
                    {
                        title = "🔨 Usuário banido",
                        color = 0xFF0000,
                        fields = new[]
                        {
                            new { name = "Usuário", value = req.Username ?? "N/A", inline = true },
                            new { name = "IP", value = req.IP ?? "N/A", inline = true },
                            new { name = "HWID", value = req.HWID ?? "N/A", inline = true },
                            new { name = "Banir usuário?", value = req.BanUser.ToString(), inline = true },
                            new { name = "Banir IP?", value = req.BanIP.ToString(), inline = true },
                            new { name = "Banir HWID?", value = req.BanHWID.ToString(), inline = true },
                            new { name = "Motivo", value = req.Reason ?? "N/A", inline = false },
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

    public class BanRequest
    {
        public string? Username { get; set; }
        public string? IP { get; set; }
        public string? HWID { get; set; }
        public bool BanUser { get; set; }
        public bool BanIP { get; set; }
        public bool BanHWID { get; set; }
        public string Reason { get; set; } = "Motivo não informado";
    }
}
