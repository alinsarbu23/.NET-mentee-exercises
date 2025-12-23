using AirportTool.Application.Services.Schedules;
using AirportTool.Infrastructure.DTOs.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public SchedulesController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET /api/schedules/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetScheduleByIdDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleService.GetByIdAsync(id, cancellationToken);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        // GET /api/schedules/stats/upcoming
        [HttpGet("stats/upcoming")]
        public async Task<ActionResult<IReadOnlyList<ScheduleStatsDto>>> GetUpcomingStats(
            CancellationToken cancellationToken)
        {
            var stats = await _scheduleService.GetUpcomingStatsAsync(cancellationToken);
            return Ok(stats);
        }

        // POST /api/schedules
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateScheduleDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _scheduleService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                null);
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File missing.");
            }
                
            if (file.Length > 2 * 1024 * 1024)
            {
                return BadRequest("File too large.");
            }
                
            using var stream = file.OpenReadStream();
            var result = await _scheduleService.ImportAsync(stream, cancellationToken);

            if (result.Errors.Any())
            {
                return StatusCode(207, result);
            }
                
            return Created(string.Empty, result);
        }

    }
}
