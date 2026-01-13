using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Airline")]
[Index("IATACode", Name = "IX_Airline_IATACode", IsUnique = true)]
[Index("IATACode", Name = "UQ_Airline_IATACode", IsUnique = true)]
public partial class AirlineDAO
{
    [Key]
    public int Id { get; set; }

    [StringLength(2)]
    public string IATACode { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Airline")]
    public virtual ICollection<FlightDAO> Flights { get; set; } = new List<FlightDAO>();
}
