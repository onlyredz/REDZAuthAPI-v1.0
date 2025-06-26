using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace REDZAuthApi.Models
{
    public class License
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string? UsedBy { get; set; }
        public string? UsedIP { get; set; }
        public string? UsedHWID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}