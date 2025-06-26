namespace REDZAuthApi.Models
{
    public class BlacklistEntry
    {
        public string Id { get; set; } = null!;
        public string? Username { get; set; }
        public string? IP { get; set; }
        public string? HWID { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
