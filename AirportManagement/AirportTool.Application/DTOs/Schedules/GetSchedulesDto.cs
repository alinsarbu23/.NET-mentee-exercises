namespace AirportTool.Infrastructure.DTOs.Schedules
{
    public class GetSchedulesDto
    {
        public List<GetScheduleByIdDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

}
