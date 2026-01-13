namespace AirportTool.Infrastructure.DTOs.Bookings
{
    public class CreateBookingDto
    {
        public int FlightScheduleId { get; set; }
        public long TicketId { get; set; }
        public string PassengerFullName { get; set; } = string.Empty;
        public string PassengerEmail { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
