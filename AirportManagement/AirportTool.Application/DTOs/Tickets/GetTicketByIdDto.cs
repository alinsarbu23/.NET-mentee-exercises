namespace AirportTool.Infrastructure.DTOs.Tickets
{
    public class GetTicketByIdDto
    {
        public long Id { get; set; }
        public int FlightScheduleId { get; set; }
        public string FareClass { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool IsRefundable { get; set; }
        public int SeatInventory { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
    }
}
