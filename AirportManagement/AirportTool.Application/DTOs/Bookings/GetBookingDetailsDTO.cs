namespace AirportTool.Application.DTOs.Bookings
{
    public class GetBookingDetailsDto
    {
        public string ConfirmationCode { get; set; } = "";
        public string Status { get; set; } = "";
        public int Quantity { get; set; }
        public DateTime CreatedUtc { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "";
    }
}
