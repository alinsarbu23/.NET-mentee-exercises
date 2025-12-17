using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IReadOnlyList<Flight>> SearchAsync(
            string? originIata,
            string? destinationIata,
            DateTime? departureDateUtc,
            string? airlineIata,
            CancellationToken cancellationToken = default);
    }
}
