using MongoDB.Bson;
using MongoDB.Driver;
using REDZAuthApi.Models;

namespace REDZAuthApi.Services
{
    public class BlacklistService
    {
        private readonly IMongoCollection<BlacklistEntry> _blacklist;

        public BlacklistService(IMongoDatabase database)
        {
            _blacklist = database.GetCollection<BlacklistEntry>("Blacklist");
        }

        public async Task<bool> IsBlacklisted(string? username, string? ip, string? hwid)
        {
            try
            {
                var filterBuilder = Builders<BlacklistEntry>.Filter;
                var filters = new List<FilterDefinition<BlacklistEntry>>();

                if (!string.IsNullOrEmpty(username))
                    filters.Add(filterBuilder.Eq(e => e.Username, username));

                if (!string.IsNullOrEmpty(ip))
                    filters.Add(filterBuilder.Eq(e => e.IP, ip));

                if (!string.IsNullOrEmpty(hwid))
                    filters.Add(filterBuilder.Eq(e => e.HWID, hwid));

                if (filters.Count == 0)
                    return false;

                var filter = filterBuilder.Or(filters);
                return await _blacklist.Find(filter).AnyAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking blacklist: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetBlacklistReason(string? username, string? ip, string? hwid)
        {
            try
            {
                var filterBuilder = Builders<BlacklistEntry>.Filter;
                var filters = new List<FilterDefinition<BlacklistEntry>>();

                if (!string.IsNullOrEmpty(username))
                    filters.Add(filterBuilder.Eq(e => e.Username, username));

                if (!string.IsNullOrEmpty(ip))
                    filters.Add(filterBuilder.Eq(e => e.IP, ip));

                if (!string.IsNullOrEmpty(hwid))
                    filters.Add(filterBuilder.Eq(e => e.HWID, hwid));

                if (filters.Count == 0)
                    return "Unknown";

                var filter = filterBuilder.Or(filters);
                var entry = await _blacklist.Find(filter).FirstOrDefaultAsync();

                return entry?.Reason ?? "Unknown";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting blacklist reason: {ex.Message}");
                return "Unknown";
            }
        }

        public async Task BanAsync(string? username, string? ip, string? hwid, string reason)
        {
            try
            {
                var entry = new BlacklistEntry
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Username = username,
                    IP = ip,
                    HWID = hwid,
                    Reason = reason,
                    Date = DateTime.UtcNow
                };

                await _blacklist.InsertOneAsync(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to blacklist: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UnbanAsync(string? username, string? ip, string? hwid)
        {
            try
            {
                var filterBuilder = Builders<BlacklistEntry>.Filter;
                var filters = new List<FilterDefinition<BlacklistEntry>>();

                if (!string.IsNullOrEmpty(username))
                    filters.Add(filterBuilder.Eq(e => e.Username, username));

                if (!string.IsNullOrEmpty(ip))
                    filters.Add(filterBuilder.Eq(e => e.IP, ip));

                if (!string.IsNullOrEmpty(hwid))
                    filters.Add(filterBuilder.Eq(e => e.HWID, hwid));

                if (filters.Count == 0)
                    return false;

                var filter = filterBuilder.Or(filters);
                var result = await _blacklist.DeleteManyAsync(filter);

                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing from blacklist: {ex.Message}");
                return false;
            }
        }
    }
}
