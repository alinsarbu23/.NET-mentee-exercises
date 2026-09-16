namespace AirportTool.Infrastructure.DTOs.Tickets
{
    public class GetTicketsDto
    {
        public List<GetTicketByIdDto> Items { get; set; } = new();
    }

}
