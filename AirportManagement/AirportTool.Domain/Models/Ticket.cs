using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Ticket")]
[Index("FlightScheduleId", "FareClass", Name = "IX_Ticket_FlightSchedule_FareClass")]
public partial class Ticket
{
    [Key]
    public long Id { get; set; }

    public long BookingId { get; set; }

    public int FlightScheduleId { get; set; }

    [StringLength(2)]
    public string FareClass { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal BasePrice { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Taxes { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [StringLength(3)]
    public string Currency { get; set; } = null!;

    public bool IsRefundable { get; set; }

    public int SeatInventory { get; set; }

    [StringLength(10)]
    public string? SeatNumber { get; set; }

    [StringLength(120)]
    public string? PassengerFullName { get; set; }

    [StringLength(120)]
    public string? PassengerEmail { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("Tickets")]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey("FlightScheduleId")]
    [InverseProperty("Tickets")]
    public virtual FlightSchedule FlightSchedule { get; set; } = null!;
}
