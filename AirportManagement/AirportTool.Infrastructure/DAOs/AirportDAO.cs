using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Airport")]
[Index("IATACode", Name = "IX_Airport_IATACode", IsUnique = true)]
[Index("IATACode", Name = "UQ_Airport_IATACode", IsUnique = true)]
public partial class AirportDAO
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
    public virtual AddressDAO Address { get; set; } = null!;

    [InverseProperty("DestinationAirport")]
    public virtual ICollection<FlightDAO> FlightDestinationAirports { get; set; } = new List<FlightDAO>();

    [InverseProperty("OriginAirport")]
    public virtual ICollection<FlightDAO> FlightOriginAirports { get; set; } = new List<FlightDAO>();

    [InverseProperty("Airport")]
    public virtual ICollection<GateDAO> Gates { get; set; } = new List<GateDAO>();
}
