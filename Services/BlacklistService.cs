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

        public async Task<string> GetBlacklistReason(string? username, string? ip, string? hwid)
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
                return "Desconhecido";

            var filter = filterBuilder.Or(filters);
            var entry = await _blacklist.Find(filter).FirstOrDefaultAsync();

            return entry?.Reason ?? "Desconhecido";
        }

        public async Task BanAsync(string? username, string? ip, string? hwid, string reason)
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

        public async Task<bool> UnbanAsync(string? username, string? ip, string? hwid)
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
    }
}
