namespace AirportTool.Infrastructure.DTOs.Flights
{
    public class GetFlightsDto
    {
        public List<GetFlightByIdDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

}
