using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace REDZAuthApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string? HWID { get; set; }
        public string? IP { get; set; }
        public string Plan { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}