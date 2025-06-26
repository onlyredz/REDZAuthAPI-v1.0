namespace REDZAuthApi.Models
{
    public class LicenseKey
    {
        public string Id { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Plan { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string? UsedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Redeemed { get; set; } = false;
    }
}
