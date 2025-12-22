using AirportTool.Application.Interfaces;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightRepository : RepositoryBase<Flight, FlightDAO>, IFlightRepository
    {
        public FlightRepository(AirportDbContext context, IMapper mapper) : base(context, mapper, context.Flights)
        {

        }

        public async Task<IReadOnlyList<Flight>> SearchAsync(string? originIata, string? destinationIata, DateTime? departureDateUtc, string? airlineIata, CancellationToken cancellationToken = default)
        {
            var query = context.Flights.Include(flight => flight.Airline)
                                       .Include(flight => flight.OriginAirport)
                                       .Include(flight => flight.DestinationAirport)
                                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(originIata))
            {
                query = query.Where(flight=>flight.OriginAirport.IATACode == originIata);
            }

            if(!string.IsNullOrWhiteSpace(destinationIata))
            {
                query = query.Where(flight => flight.DestinationAirport.IATACode == destinationIata);
            }

            if(!string.IsNullOrWhiteSpace(airlineIata))
            {
                query = query.Where(flight => flight.Airline.IATACode == airlineIata);
            }

            var daos = await query.ToListAsync(cancellationToken);

            return mapper.Map<List<Flight>>(daos);

        }

        public async Task<Airline?> GetAirlineByIataAsync(
    string iata,
    CancellationToken cancellationToken = default)
        {
            var dao = await context.Airlines
                .FirstOrDefaultAsync(a => a.IATACode == iata, cancellationToken);

            return dao == null ? null : mapper.Map<Airline>(dao);
        }

        public async Task<Airport?> GetAirportByIataAsync(
            string iata,
            CancellationToken cancellationToken = default)
        {
            var dao = await context.Airports
                .FirstOrDefaultAsync(a => a.IATACode == iata, cancellationToken);

            return dao == null ? null : mapper.Map<Airport>(dao);
        }

        public async Task<Flight?> GetByKeyAsync(
            int airlineId,
            string flightNumber,
            int originAirportId,
            int destinationAirportId,
            CancellationToken cancellationToken = default)
        {
            var dao = await context.Flights.FirstOrDefaultAsync(f =>
                f.AirlineId == airlineId &&
                f.FlightNumber == flightNumber &&
                f.OriginAirportId == originAirportId &&
                f.DestinationAirportId == destinationAirportId,
                cancellationToken);

            return dao == null ? null : mapper.Map<Flight>(dao);
        }


    }
}


