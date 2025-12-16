using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Airport")]
[Index("IATACode", Name = "IX_Airport_IATACode", IsUnique = true)]
[Index("IATACode", Name = "UQ_Airport_IATACode", IsUnique = true)]
public partial class Airport
{
    [Key]
    public int Id { get; set; }

    [StringLength(3)]
    public string IATACode { get; set; } = null!;

    [StringLength(120)]
    public string Name { get; set; } = null!;

    [StringLength(64)]
    public string TimeZone { get; set; } = null!;

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Airports")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("DestinationAirport")]
    public virtual ICollection<Flight> FlightDestinationAirports { get; set; } = new List<Flight>();

    [InverseProperty("OriginAirport")]
    public virtual ICollection<Flight> FlightOriginAirports { get; set; } = new List<Flight>();

    [InverseProperty("Airport")]
    public virtual ICollection<Gate> Gates { get; set; } = new List<Gate>();
}
