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

    public async Task<Ticket?> GetOfferByIdAsync(long ticketId, CancellationToken ct = default)
    {
        var dao = await context.Tickets
            .AsTracking()
            .FirstOrDefaultAsync(t =>
                t.Id == ticketId
                && t.SeatInventory > 0
                && t.SeatNumber == null
                && t.PassengerFullName == null
                && t.PassengerEmail == null,
                ct);

        return dao == null ? null : mapper.Map<Ticket>(dao);
    }



    public async Task AddRangeAsync(IEnumerable<Ticket> tickets, CancellationToken ct = default)
    {
        var daos = mapper.Map<List<TicketDAO>>(tickets);
        await context.Tickets.AddRangeAsync(daos, ct);
    }

    public async Task<IReadOnlyList<Ticket>> GetSoldByBookingIdAsync(long bookingId, CancellationToken ct = default)
    {
        var daos = await context.Tickets
            .AsTracking()
            .Where(t => t.BookingId == bookingId)
            .ToListAsync(ct);

        return mapper.Map<List<Ticket>>(daos);
    }

    public async Task DecrementOfferInventoryAsync(long ticketId, int quantity, CancellationToken ct = default)
    {
        var dao = await context.Tickets
            .AsTracking()
            .FirstOrDefaultAsync(t =>
                t.Id == ticketId
                && t.SeatInventory > 0
                && t.SeatNumber == null
                && t.PassengerFullName == null
                && t.PassengerEmail == null,
                ct);

        if (dao == null) throw new KeyNotFoundException("Ticket offer not found.");

        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (quantity > dao.SeatInventory) throw new ArgumentException("Not enough seats available.");

        dao.SeatInventory -= quantity; 
    }

    public async Task IncrementOfferInventoryAsync(int flightScheduleId, string fareClass, int quantity, CancellationToken ct = default)
    {
        var dao = await context.Tickets
            .AsTracking()
            .FirstOrDefaultAsync(t =>
                t.FlightScheduleId == flightScheduleId
                && t.FareClass == fareClass
                && t.SeatInventory >= 0
                && t.SeatNumber == null
                && t.PassengerFullName == null
                && t.PassengerEmail == null,
                ct);

        if (dao == null)
            throw new InvalidOperationException("Offer row not found for restore inventory.");

        dao.SeatInventory += quantity;
    }

    public async Task<int> CountSoldSeatsForScheduleAsync(int flightScheduleId, CancellationToken ct = default)
    {
        return await context.Tickets
            .AsNoTracking()
            .CountAsync(t =>
                t.FlightScheduleId == flightScheduleId
                && (t.SeatNumber != null || t.PassengerFullName != null || t.PassengerEmail != null),
                ct);
    }

}
