namespace AirportTool.Infrastructure.DTOs.Flights
{
    public class CreateFlightDto
    {
        public string AirlineIata { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public string OriginIata { get; set; } = string.Empty;
        public string DestinationIata { get; set; } = string.Empty;
        public string? DefaultAircraftTail { get; set; }
    }

}
