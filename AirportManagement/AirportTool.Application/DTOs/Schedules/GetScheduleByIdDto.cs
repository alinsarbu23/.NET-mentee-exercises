namespace AirportTool.Infrastructure.DTOs.Schedules
{
    public class GetScheduleByIdDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public string? GateCode { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
