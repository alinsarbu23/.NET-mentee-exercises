using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportTool.Infrastructure.Data.Models;

[Table("FlightStatus")]
public partial class FlightStatusDAO
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [InverseProperty("FlightStatus")]
    public virtual ICollection<FlightScheduleDAO> FlightSchedules { get; set; } = new List<FlightScheduleDAO>();
}
