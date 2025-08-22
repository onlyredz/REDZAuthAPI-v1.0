using Microsoft.Extensions.Options;
using MongoDB.Driver;
using REDZAuthApi.Models;

namespace REDZAuthApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB") 
                ?? throw new InvalidOperationException("MongoDB connection string not found");
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("REDZAuth");
        }

        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");

        public IMongoCollection<License> Licenses =>
            _database.GetCollection<License>("Licenses");

        public IMongoCollection<BlacklistEntry> Blacklist =>
            _database.GetCollection<BlacklistEntry>("Blacklist");
    }
}
