using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("FlightSchedule")]
[Index("FlightId", "ScheduledDepartureUtc", Name = "IX_FlightSchedule_Flight_Departure")]
public partial class FlightScheduleDAO
{
    [Key]
    public int Id { get; set; }

    public int FlightId { get; set; }

    [Precision(0)]
    public DateTime ScheduledDepartureUtc { get; set; }

    [Precision(0)]
    public DateTime ScheduledArrivalUtc { get; set; }

    public int? GateId { get; set; }

    public int? AssignedAircraftId { get; set; }

    public int FlightStatusId { get; set; }

    [ForeignKey("AssignedAircraftId")]
    [InverseProperty("FlightSchedules")]
    public virtual AircraftDAO? AssignedAircraft { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("FlightSchedules")]
    public virtual FlightDAO Flight { get; set; } = null!;

    [ForeignKey("FlightStatusId")]
    [InverseProperty("FlightSchedules")]
    public virtual FlightStatusDAO FlightStatus { get; set; } = null!;

    [ForeignKey("GateId")]
    [InverseProperty("FlightSchedules")]
    public virtual GateDAO? Gate { get; set; }

    [InverseProperty("FlightSchedule")]
    public virtual ICollection<TicketDAO> Tickets { get; set; } = new List<TicketDAO>();
}
