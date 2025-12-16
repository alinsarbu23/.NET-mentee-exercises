using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Booking")]
[Index("ConfirmationCode", Name = "IX_Booking_ConfirmationCode", IsUnique = true)]
[Index("ConfirmationCode", Name = "UQ_Booking_ConfirmationCode", IsUnique = true)]
public partial class Booking
{
    [Key]
    public long Id { get; set; }

    public int UserId { get; set; }

    public int BookingStatusId { get; set; }

    [Precision(0)]
    public DateTime CreatedUtc { get; set; }

    [StringLength(8)]
    public string ConfirmationCode { get; set; } = null!;

    public int Quantity { get; set; }

    [ForeignKey("BookingStatusId")]
    [InverseProperty("Bookings")]
    public virtual BookingStatus BookingStatus { get; set; } = null!;

    [InverseProperty("Booking")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    [ForeignKey("UserId")]
    [InverseProperty("Bookings")]
    public virtual User User { get; set; } = null!;
}
