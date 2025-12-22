namespace AirportTool.Application.DTOs.Schedules
{
    public class ImportScheduleErrorDto
    {
        public int Row { get; set; }
        public string Message { get; set; } = null!;
    }
}
