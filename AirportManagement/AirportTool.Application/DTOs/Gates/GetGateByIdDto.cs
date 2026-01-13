namespace AirportTool.Infrastructure.DTOs.Gates
{
    public class GetGateByIdDto
    {
        public int Id { get; set; }
        public int AirportId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
