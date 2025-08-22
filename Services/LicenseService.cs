using MongoDB.Driver;
using REDZAuthApi.Models;

namespace REDZAuthApi.Services
{
    public class LicenseService
    {
        private readonly IMongoCollection<License> _licenseCollection;

        public LicenseService(IMongoDatabase database)
        {
            _licenseCollection = database.GetCollection<License>("Licenses");
        }

        private int GetDaysForPlan(string plan)
        {
            return plan.ToLower() switch
            {
                "mensal" => 30,
                "trimestral" => 91,
                "anual" => 366,
                "lifetime" => 9999,
                _ => 30
            };
        }

        public string GenerateAutoKey(string plan)
        {
            var prefix = plan.ToUpper() switch
            {
                "MENSAL" => "MENSAL",
                "TRIMESTRAL" => "TRIMES",
                "ANUAL" => "ANUAL",
                "LIFETIME" => "LIFE",
                _ => "GEN"
            };

            var random = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"{prefix}-{random[..4]}-{random[4..]}";
        }

        public async Task<License> CreateLicenseAsync(string plan)
        {
            try
            {
                var key = GenerateAutoKey(plan);
                var expiration = DateTime.UtcNow.AddDays(GetDaysForPlan(plan));

                var license = new License
                {
                    Key = key,
                    Plan = plan,
                    Expiration = expiration
                };

                await _licenseCollection.InsertOneAsync(license);
                return license;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating license: {ex.Message}");
                throw;
            }
        }

        public async Task<(bool Success, string? Error)> CreateCustomLicenseAsync(string customKey, string plan)
        {
            try
            {
                var exists = await _licenseCollection.Find(l => l.Key == customKey).FirstOrDefaultAsync();
                if (exists != null)
                    return (false, "Key already exists");

                var expiration = DateTime.UtcNow.AddDays(GetDaysForPlan(plan));

                var license = new License
                {
                    Key = customKey,
                    Plan = plan,
                    Expiration = expiration
                };

                await _licenseCollection.InsertOneAsync(license);
                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating custom license: {ex.Message}");
                return (false, "Failed to create license");
            }
        }

        public async Task<License?> GetLicenseByKeyAsync(string key)
        {
            try
            {
                return await _licenseCollection.Find(l => l.Key == key).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting license by key: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            try
            {
                return await _licenseCollection.Find(l => l.Key == key).AnyAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if key exists: {ex.Message}");
                return false;
            }
        }

        public async Task<List<License>> GetFilteredKeysAsync(FilterDefinition<License> filter)
        {
            try
            {
                return await _licenseCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting filtered keys: {ex.Message}");
                return new List<License>();
            }
        }

        public async Task<bool> MarkLicenseAsUsedAsync(string key, string username, string ip, string hwid)
        {
            try
            {
                var update = Builders<License>.Update
                    .Set(l => l.UsedBy, username)
                    .Set(l => l.UsedIP, ip)
                    .Set(l => l.UsedHWID, hwid);

                var result = await _licenseCollection.UpdateOneAsync(
                    l => l.Key == key && l.UsedBy == null,
                    update
                );

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking license as used: {ex.Message}");
                return false;
            }
        }
    }
}
