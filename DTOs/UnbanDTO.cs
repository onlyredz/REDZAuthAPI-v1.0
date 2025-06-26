namespace REDZAuthApi.DTOs
{
    public class UnbanDTO
    {
        public string? Username { get; set; }
        public string? IP { get; set; }
        public string? HWID { get; set; }
        public bool UnbanUser { get; set; }
        public bool UnbanIP { get; set; }
        public bool UnbanHWID { get; set; }
        public string Reason { get; set; } = "Motivo não informado";
    }
}
