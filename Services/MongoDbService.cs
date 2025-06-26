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
            var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users =>
            _database.GetCollection<User>("Users");
    }
}
