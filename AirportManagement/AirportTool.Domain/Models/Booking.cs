namespace AirportTool.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingStatusId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string ConfirmationCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
