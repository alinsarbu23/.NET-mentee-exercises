namespace AirportTool.Infrastructure.DTOs.Aircraft
{
    public class CreateAircraftDto
    {
        public string TailNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int SeatCapacity { get; set; }
    }
}
