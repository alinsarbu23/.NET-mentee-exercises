namespace AirportTool.Application.DTOs.Schedules
{
    public class ImportScheduleResultDto
    {
        public int Total { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }

        public List<ImportScheduleErrorDto> Errors { get; set; } = new();
    }
}
