using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Index("TailNumber", Name = "UQ_Aircraft_TailNumber", IsUnique = true)]
public partial class AircraftDAO
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string TailNumber { get; set; } = null!;

    [StringLength(60)]
    public string Model { get; set; } = null!;

    public int SeatCapacity { get; set; }

    [InverseProperty("AssignedAircraft")]
    public virtual ICollection<FlightScheduleDAO> FlightSchedules { get; set; } = new List<FlightScheduleDAO>();

    [InverseProperty("DefaultAircraft")]
    public virtual ICollection<FlightDAO> Flights { get; set; } = new List<FlightDAO>();
}
