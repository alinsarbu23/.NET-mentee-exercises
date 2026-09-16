using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface IFlightRepository : IRepository<Flight, int>
    {
        Task<IReadOnlyList<Flight>> SearchAsync(
            string? originIata,
            string? destinationIata,
            DateTime? departureDateUtc,
            string? airlineIata,
            CancellationToken cancellationToken = default);

        Task<Airline?> GetAirlineByIataAsync(
           string iata,
           CancellationToken cancellationToken = default);

        Task<Airport?> GetAirportByIataAsync(
            string iata,
            CancellationToken cancellationToken = default);

        Task<Flight?> GetByKeyAsync(
            int airlineId,
            string flightNumber,
            int originAirportId,
            int destinationAirportId,
            CancellationToken cancellationToken = default);
    }
}
