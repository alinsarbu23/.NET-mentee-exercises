namespace AirportTool.Domain.Models
{

    public class Airline
    {
        public int Id { get; set; }
        public string IATACode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

}
