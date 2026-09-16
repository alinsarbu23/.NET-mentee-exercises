using AirportTool.Application.Services.Schedules;
using AirportTool.Infrastructure.DTOs.Schedules;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetScheduleByIdDto>> GetById(int id, CancellationToken ct)
        {
            var result = await _scheduleService.GetByIdAsync(id, ct);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "Staff, User")]
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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateScheduleDto dto, CancellationToken ct)
        {
            await _scheduleService.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [Authorize(Roles = "Staff")]
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
