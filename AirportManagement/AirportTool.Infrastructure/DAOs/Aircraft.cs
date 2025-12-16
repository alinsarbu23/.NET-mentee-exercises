using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Index("TailNumber", Name = "UQ_Aircraft_TailNumber", IsUnique = true)]
public partial class Aircraft
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string TailNumber { get; set; } = null!;

    [StringLength(60)]
    public string Model { get; set; } = null!;

    public int SeatCapacity { get; set; }

    [InverseProperty("AssignedAircraft")]
    public virtual ICollection<FlightSchedule> FlightSchedules { get; set; } = new List<FlightSchedule>();

    [InverseProperty("DefaultAircraft")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
