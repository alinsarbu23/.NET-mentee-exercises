namespace AirportTool.Application.DTOs.Flights
{
    /// <summary>
    /// for searching flights with optional parameters. (Body)
    /// </summary>
    public class GetFlightSearchDto 
    {
        public string? OriginIata { get; set; }
        public string? DestinationIata { get; set; }
        public DateOnly? Date { get; set; }
        public string? AirlineIata { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

    }
}
