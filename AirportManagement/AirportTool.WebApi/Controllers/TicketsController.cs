using AirportTool.Application.Services.Tickets;
using AirportTool.Infrastructure.DTOs.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET /api/tickets/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetTicketByIdDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketService.GetByIdAsync(id, cancellationToken);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // GET /api/tickets/by-schedule/{flightScheduleId}
        [HttpGet("by-schedule/{flightScheduleId:int}")]
        public async Task<ActionResult<IReadOnlyList<GetTicketByIdDto>>> GetByFlightSchedule(
            int flightScheduleId,
            CancellationToken cancellationToken)
        {
            var tickets = await _ticketService.GetByFlightScheduleAsync(
                flightScheduleId,
                cancellationToken);

            return Ok(tickets);
        }

        // POST /api/tickets
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateTicketDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _ticketService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                null);
        }

        // PUT /api/tickets/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(
            int id,
            [FromBody] UpdateTicketDto dto,
            CancellationToken cancellationToken)
        {
            await _ticketService.UpdateAsync(id, dto, cancellationToken);
            return NoContent();
        }

        // DELETE /api/tickets/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _ticketService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
