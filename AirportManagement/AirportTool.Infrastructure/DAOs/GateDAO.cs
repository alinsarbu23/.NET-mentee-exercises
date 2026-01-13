using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Gate")]
[Index("AirportId", "Code", Name = "IX_Gate_Airport_Code", IsUnique = true)]
[Index("AirportId", "Code", Name = "UQ_Gate_Airport_Code", IsUnique = true)]
public partial class GateDAO
{
    [Key]
    public int Id { get; set; }

    public int AirportId { get; set; }

    [StringLength(10)]
    public string Code { get; set; } = null!;

    [ForeignKey("AirportId")]
    [InverseProperty("Gates")]
    public virtual AirportDAO Airport { get; set; } = null!;

    [InverseProperty("Gate")]
    public virtual ICollection<FlightScheduleDAO> FlightSchedules { get; set; } = new List<FlightScheduleDAO>();
}
