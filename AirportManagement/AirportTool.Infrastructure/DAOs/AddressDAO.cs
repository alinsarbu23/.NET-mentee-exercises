using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportTool.Infrastructure.Data.Models;

[Table("Address")]
public partial class AddressDAO
{
    [Key]
    public int Id { get; set; }

    [StringLength(80)]
    public string Country { get; set; } = null!;

    [StringLength(80)]
    public string City { get; set; } = null!;

    [StringLength(120)]
    public string Street { get; set; } = null!;

    [InverseProperty("Address")]
    public virtual ICollection<AirportDAO> Airports { get; set; } = new List<AirportDAO>();
}
