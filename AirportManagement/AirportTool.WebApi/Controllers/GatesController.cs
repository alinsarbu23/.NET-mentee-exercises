using AirportTool.Application.Services;
using AirportTool.Infrastructure.DTOs.Gates;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatesController : ControllerBase
    {
        private readonly GateService service;

        public GatesController(GateService service)
        {
            this.service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetGateByIdDto>> GetById(int id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateGateDto dto, CancellationToken ct)
        {
            var id = await service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateGateDto dto, CancellationToken ct)
        {
            await service.UpdateAsync(id, dto, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
