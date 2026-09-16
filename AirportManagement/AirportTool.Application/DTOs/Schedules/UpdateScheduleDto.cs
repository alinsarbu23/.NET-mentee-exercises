namespace AirportTool.Infrastructure.DTOs.Schedules
{
    public class UpdateScheduleDto
    {
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public int? GateId { get; set; }
        public int FlightStatusId { get; set; }
    }
}
