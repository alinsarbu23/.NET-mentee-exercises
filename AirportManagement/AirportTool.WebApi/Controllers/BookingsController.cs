using AirportTool.Application.Services.Bookings;
using AirportTool.Infrastructure.DTOs.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingsController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateBookingDto dto,
            CancellationToken cancellationToken)
        {
            var confirmationCode = await _bookingService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetByCode),
                new { code = confirmationCode },
                new { confirmationCode = confirmationCode });
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<GetBookingByIdDto>> GetByCode(
            string code,
            CancellationToken cancellationToken)
        {
            var booking = await _bookingService.GetByCodeAsync(code, cancellationToken);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> Cancel(
            string code,
            CancellationToken cancellationToken)
        {
            await _bookingService.CancelAsync(code, cancellationToken);
            return NoContent();
        }
    }
}
