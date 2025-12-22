using AirportTool.Application.Services.Flights;
using AirportTool.Infrastructure.DTOs.Flights;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        // GET /api/flights/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetFlightByIdDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var flight = await _flightService.GetByIdAsync(id, cancellationToken);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        // GET /api/flights?originIata=OTP&destinationIata=LHR&date=2025-12-01&airlineIata=RO
        //[HttpGet]
        //public async Task<ActionResult<IReadOnlyList<GetFlightByIdDto>>> Search(
        //    [FromQuery] string? originIata,
        //    [FromQuery] string? destinationIata,
        //    [FromQuery] DateTime? date,
        //    [FromQuery] string? airlineIata,
        //    CancellationToken cancellationToken)
        //{
        //    var flights = await _flightService.SearchAsync(
        //        originIata,
        //        destinationIata,
        //        date,
        //        airlineIata,
        //        cancellationToken);

        //    return Ok(flights);
        //}

        // POST /api/flights
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateFlightDto dto,
            CancellationToken cancellationToken)
        {
            var id = await _flightService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                null);
        }

        // PUT /api/flights/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(
            int id,
            [FromBody] UpdateFlightDto dto,
            CancellationToken cancellationToken)
        {
            await _flightService.UpdateAsync(id, dto, cancellationToken);
            return NoContent();
        }

        // DELETE /api/flights/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _flightService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
