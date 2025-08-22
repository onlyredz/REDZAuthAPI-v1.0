using MongoDB.Driver;
using REDZAuthApi.DTOs;
using REDZAuthApi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace REDZAuthApi.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<License> _licenses;
        private readonly BlacklistService _blacklistService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(IMongoDatabase database, BlacklistService blacklistService, HttpClient httpClient, IConfiguration configuration)
        {
            _users = database.GetCollection<User>("Users");
            _licenses = database.GetCollection<License>("Licenses");
            _blacklistService = blacklistService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<(User?, string?)> RegisterAsync(RegisterDTO dto, HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var existingUser = await _users.Find(u => u.Username == dto.Username).FirstOrDefaultAsync();
            if (existingUser != null)
                return (null, "Failed to Authenticate: Username already exists");

            var license = await _licenses.Find(k => k.Key == dto.Key && k.UsedBy == null).FirstOrDefaultAsync();
            if (license == null)
                return (null, "Failed to Authenticate: Invalid or already used license key");

            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                HWID = dto.HWID,
                IP = ip,
                Plan = license.Plan,
                Expiration = license.Expiration
            };

            await _users.InsertOneAsync(newUser);

            var update = Builders<License>.Update
                .Set(k => k.UsedBy, dto.Username)
                .Set(k => k.UsedHWID, dto.HWID)
                .Set(k => k.UsedIP, ip);

            await _licenses.UpdateOneAsync(k => k.Id == license.Id, update);

            await SendWebhookRegister(newUser.Username, ip, newUser.HWID, newUser.Plan, newUser.Expiration);

            return (newUser, null);
        }

        public async Task<(User?, string?)> LoginAsync(LoginDTO dto, HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var currentHWID = dto.HWID;

            var user = await _users.Find(u => u.Username == dto.Username).FirstOrDefaultAsync();

            if (user == null)
                return (null, "Failed to Authenticate: Username not found");

            if (user.PasswordHash != HashPassword(dto.Password))
                return (null, "Failed to Authenticate: Invalid Password");

            if (await _blacklistService.IsBlacklisted(dto.Username, ip, currentHWID))
            {
                var reason = await _blacklistService.GetBlacklistReason(dto.Username, ip, currentHWID);
                await SendWebhookBanAttempt(dto.Username, ip, currentHWID, reason);
                return (null, $"Account Blacklisted: {reason}");
            }

            if (user.Expiration < DateTime.UtcNow)
            {
                await SendWebhookExpiration(user.Username, ip, currentHWID, user.Expiration);
                return (null, "Failed to Authenticate: License expired");
            }

            if (string.IsNullOrEmpty(user.HWID))
            {
                var update = Builders<User>.Update.Set(u => u.HWID, currentHWID);
                await _users.UpdateOneAsync(u => u.Id == user.Id, update);
                user.HWID = currentHWID;
                await SendWebhookFirstTimeHWID(user.Username, ip, currentHWID);
            }
            else if (user.HWID != currentHWID)
            {
                await SendWebhookHWID(user.Username, ip, currentHWID, user.HWID);
                return (null, "Failed to Authenticate: HWID Mismatch");
            }

            return (user, null);
        }

        public async Task<(bool Success, string Message)> ResetHWIDAsync(string username)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

            if (user == null)
                return (false, "Failed to Reset HWID: User not found");

            var update = Builders<User>.Update.Set(u => u.HWID, null);
            await _users.UpdateOneAsync(u => u.Id == user.Id, update);

            return (true, "HWID Reset Successfully");
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private async Task SendWebhookAsync(object payload)
        {
            try
            {
                var webhookUrl = _configuration["WebhookSettings:DiscordWebhookUrl"];
                if (string.IsNullOrEmpty(webhookUrl) || webhookUrl == "YOUR_DISCORD_WEBHOOK_URL_HERE")
                    return; // Skip webhook if not configured

                var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(webhookUrl, json);
            }
            catch (Exception ex)
            {
                // Log webhook error but don't fail the main operation
                Console.WriteLine($"Webhook error: {ex.Message}");
            }
        }

        private async Task SendWebhookRegister(string username, string ip, string hwid, string plan, DateTime expiration)
        {
            var embed = new
            {
                embeds = new[] {
                    new {
                        title = "📝 New Account Registered",
                        color = 0x00FFFF,
                        fields = new[] {
                            new { name = "Username", value = username, inline = true },
                            new { name = "IP", value = ip, inline = true },
                            new { name = "HWID", value = hwid, inline = false },
                            new { name = "Plan", value = plan, inline = true },
                            new {
                                name = "Expiration",
                                value = expiration > DateTime.UtcNow.AddYears(10) ? "Lifetime" : expiration.ToString("yyyy-MM-dd HH:mm:ss"),
                                inline = false
                            },
                            new { name = "Date", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };
            await SendWebhookAsync(embed);
        }

        private async Task SendWebhookBanAttempt(string username, string ip, string hwid, string reason)
        {
            var embed = new
            {
                embeds = new[] {
                    new {
                        title = "🚫 Blocked Login Attempt",
                        color = 0xFF0000,
                        fields = new[] {
                            new { name = "Username", value = username, inline = true },
                            new { name = "IP", value = ip, inline = true },
                            new { name = "HWID", value = hwid, inline = false },
                            new { name = "Reason", value = reason, inline = false },
                            new { name = "Time", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };
            await SendWebhookAsync(embed);
        }

        private async Task SendWebhookExpiration(string username, string ip, string hwid, DateTime expiration)
        {
            var embed = new
            {
                embeds = new[] {
                    new {
                        title = "⛔ Login with Expired License",
                        color = 0xFF0000,
                        fields = new[] {
                            new { name = "Username", value = username, inline = true },
                            new { name = "IP", value = ip, inline = true },
                            new { name = "HWID", value = hwid, inline = false },
                            new {
                                name = "Expiration",
                                value = expiration > DateTime.UtcNow.AddYears(10) ? "Lifetime" : expiration.ToString("yyyy-MM-dd HH:mm:ss"),
                                inline = false
                            },
                            new { name = "Attempt Time", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };
            await SendWebhookAsync(embed);
        }

        private async Task SendWebhookFirstTimeHWID(string username, string ip, string hwid)
        {
            var embed = new
            {
                embeds = new[] {
                    new {
                        title = "✅ First-time HWID Set",
                        color = 0x00FF00,
                        fields = new[] {
                            new { name = "Username", value = username, inline = true },
                            new { name = "IP", value = ip, inline = true },
                            new { name = "New HWID", value = hwid, inline = false },
                            new { name = "Date", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };
            await SendWebhookAsync(embed);
        }

        private async Task SendWebhookHWID(string username, string ip, string newHWID, string savedHWID)
        {
            var embed = new
            {
                embeds = new[] {
                    new {
                        title = "⚠️ HWID Mismatch Detected",
                        color = 0xFFA500,
                        fields = new[] {
                            new { name = "Username", value = username, inline = true },
                            new { name = "IP", value = ip, inline = true },
                            new { name = "Saved HWID", value = savedHWID, inline = false },
                            new { name = "New HWID", value = newHWID, inline = false },
                            new { name = "Time", value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), inline = false }
                        }
                    }
                }
            };
            await SendWebhookAsync(embed);
        }
    }
}
