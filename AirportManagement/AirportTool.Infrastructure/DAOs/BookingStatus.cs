using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportTool.Infrastructure.Data.Models;

[Table("BookingStatus")]
public partial class BookingStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [InverseProperty("BookingStatus")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
