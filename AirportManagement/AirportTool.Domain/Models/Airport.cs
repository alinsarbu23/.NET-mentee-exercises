namespace AirportTool.Domain.Entities;

public class Airport
{
    public int Id { get; set; }
    public string IATACode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public int AddressId { get; set; }
}
