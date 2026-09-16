namespace AirportTool.Domain.Models
{
    public class Ticket
    {
        public long Id { get; set; }
        public long? BookingId { get; set; }
        public int FlightScheduleId { get; set; }
        public string FareClass { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool IsRefundable { get; set; }
        public int SeatInventory { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string PassengerFullName { get; set; } = string.Empty;
        public string PassengerEmail { get; set; } = string.Empty;


    }
}
