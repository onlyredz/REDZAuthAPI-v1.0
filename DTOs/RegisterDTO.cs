namespace REDZAuthApi.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string HWID { get; set; } = string.Empty;
        public string IP { get; set; } = string.Empty;
    }
}