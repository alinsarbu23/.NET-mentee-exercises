namespace AirportTool.Infrastructure.DTOs.Flights
{
    public class GetFlightByIdDto
    {
        public int Id { get; set; }
        public string AirlineIata { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;

        public string OriginIata { get; set; } = string.Empty;
        public string DestinationIata { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public string? DefaultAircraftTail { get; set; }
    }

}
