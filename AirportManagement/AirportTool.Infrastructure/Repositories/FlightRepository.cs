using AirportTool.Application.Interfaces;
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
    }
}


