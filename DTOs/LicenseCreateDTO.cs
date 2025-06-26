namespace REDZAuthApi.DTOs
{
    public class LicenseCreateDTO
    {
        public string Plan { get; set; } = string.Empty;
        public int DurationDays { get; set; }
    }
}
