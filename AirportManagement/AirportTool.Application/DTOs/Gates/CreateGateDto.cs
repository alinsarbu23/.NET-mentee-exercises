namespace AirportTool.Infrastructure.DTOs.Gates
{
    public class CreateGateDto
    {
        public int AirportId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
