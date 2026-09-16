using System.ComponentModel.DataAnnotations;

namespace AirportTool.Application.DTOs.Schedules
{
    public class ImportScheduleRowDto
    {
        [Required]
        public string FlightNumber { get; set; } = null!;

        [Required]
        public string AirlineIata { get; set; } = null!;

        [Required]
        public string OriginIata { get; set; } = null!;

        [Required]
        public string DestinationIata { get; set; } = null!;

        [Required]
        public DateTime ScheduledDepartureUtc { get; set; }

        [Required]
        public DateTime ScheduledArrivalUtc { get; set; }

        public string? GateCode { get; set; }
        public string? AssignedAircraftTail { get; set; }
    }
}
