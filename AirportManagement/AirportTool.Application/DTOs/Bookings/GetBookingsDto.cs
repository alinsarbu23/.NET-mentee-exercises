namespace AirportTool.Infrastructure.DTOs.Bookings
{
    public class GetBookingsDto
    {
        public List<GetBookingByIdDto> Items { get; set; } = new();
    }

}
