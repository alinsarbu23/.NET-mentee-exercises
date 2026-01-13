namespace AirportTool.Infrastructure.DTOs.Bookings
{
    public class GetBookingByIdDto
    {
        public long Id { get; set; }
        public string ConfirmationCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PassengerFullName { get; set; } = string.Empty;
        public string PassengerEmail { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
