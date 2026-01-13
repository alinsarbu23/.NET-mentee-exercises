using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Flight")]
[Index("AirlineId", "FlightNumber", Name = "IX_Flight_Airline_FlightNumber")]
[Index("OriginAirportId", "DestinationAirportId", Name = "IX_Flight_Origin_Destination")]
public partial class FlightDAO
{
    [Key]
    public int Id { get; set; }

    public int AirlineId { get; set; }

    [StringLength(8)]
    public string FlightNumber { get; set; } = null!;

    public int OriginAirportId { get; set; }

    public int DestinationAirportId { get; set; }

    public bool IsActive { get; set; }

    public int? DefaultAircraftId { get; set; }

    [ForeignKey("AirlineId")]
    [InverseProperty("Flights")]
    public virtual AirlineDAO Airline { get; set; } = null!;

    [ForeignKey("DefaultAircraftId")]
    [InverseProperty("Flights")]
    public virtual AircraftDAO? DefaultAircraft { get; set; }

    [ForeignKey("DestinationAirportId")]
    [InverseProperty("FlightDestinationAirports")]
    public virtual AirportDAO DestinationAirport { get; set; } = null!;

    [InverseProperty("Flight")]
    public virtual ICollection<FlightScheduleDAO> FlightSchedules { get; set; } = new List<FlightScheduleDAO>();

    [ForeignKey("OriginAirportId")]
    [InverseProperty("FlightOriginAirports")]
    public virtual AirportDAO OriginAirport { get; set; } = null!;
}
