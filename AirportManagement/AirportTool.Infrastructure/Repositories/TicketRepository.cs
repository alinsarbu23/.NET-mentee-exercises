using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure;
using AirportTool.Infrastructure.Data.Models;
using AirportTool.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class TicketRepository
    : RepositoryBase<Ticket, TicketDAO, long>, ITicketRepository
{
    public TicketRepository(AirportDbContext context, IMapper mapper): base(context, mapper, context.Tickets)
    {
        
    }

    public async Task<IReadOnlyList<Ticket>> GetByFlightScheduleIdAsync(
        int flightScheduleId,
        CancellationToken cancellationToken = default)
    {
        var daos = await context.Tickets
            .Where(ticket => ticket.FlightScheduleId == flightScheduleId)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Ticket>>(daos);
    }
}
