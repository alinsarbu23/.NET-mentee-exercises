using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class TicketRepository: RepositoryBase<Ticket, TicketDAO>, ITicketRepository
    {
        public TicketRepository(AirportDbContext context, IMapper mapper): base(context, mapper, context.Tickets)
        {
        }

        public async Task<IReadOnlyList<Ticket>> GetByFlightScheduleIdAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            var daos = await context.Tickets.Where(ticket => ticket.FlightScheduleId == flightScheduleId).ToListAsync(cancellationToken);

            return mapper.Map<List<Ticket>>(daos);
        }
    }
}
