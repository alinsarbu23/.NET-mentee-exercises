namespace AirportTool.Application.DTOs.Flights
{
    /// <summary>
    /// For getting flights with details in search results. 
    /// </summary>
    public class FlightSearchResultDto
    {
        public int ScheduleId { get; set; }
        public string AirlineIata { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;

        public string OriginIata { get; set; } = string.Empty;
        public string DestinationIata { get; set; } = string.Empty;

        public DateTime ScheduledDepartureUtc { get; set; } 
        public DateTime ScheduledArrivalUtc { get; set; } 

        public string? GateCode { get; set; }
        public string Status { get; set; } = string.Empty; // Planned/Boarding etc.
    }
}
