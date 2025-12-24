using AirportTool.Application.Services.Tickets;
using AirportTool.Infrastructure.DTOs.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService ticketService;

        public TicketsController(TicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [HttpGet("by-schedule/{flightScheduleId:int}")]
        public async Task<ActionResult<IReadOnlyList<GetTicketByIdDto>>> GetBySchedule(
            int flightScheduleId,
            CancellationToken cancellationToken)
        {
            var offers = await ticketService.GetOffersByScheduleAsync(flightScheduleId, cancellationToken);
            return Ok(offers);
        }

        [AllowAnonymous]
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetTicketByIdDto>> GetOfferById(long id, CancellationToken cancellationToken)
        {
            var offer = await ticketService.GetOfferByIdAsync(id, cancellationToken);
            if (offer == null) return NotFound();
            return Ok(offer);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTicketDto dto, CancellationToken cancellationToken)
        {
            var id = await ticketService.CreateOfferAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetOfferById),
                new { id },
                null);
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("{id:long}")]
        public async Task<ActionResult> Update(long id, [FromBody] UpdateTicketDto dto, CancellationToken cancellationToken)
        {
            await ticketService.UpdateInventoryAsync(id, dto, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            await ticketService.DeleteOfferAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
