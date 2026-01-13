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

    public async Task<Ticket?> GetOfferByIdAsync(long ticketId, CancellationToken cancellationToken = default)
    {
        var offersBookingId = await context.Bookings
            .AsNoTracking()
            .Where(b => b.ConfirmationCode == "OFFERS00")
            .Select(b => b.Id)
            .SingleAsync(cancellationToken);

        var dao = await context.Tickets
            .AsTracking()
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.BookingId == offersBookingId, cancellationToken);

        return dao == null ? null : mapper.Map<Ticket>(dao);
    }

    public async Task AddRangeAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default)
    {
        var daos = mapper.Map<List<TicketDAO>>(tickets);
        await context.Tickets.AddRangeAsync(daos, cancellationToken);
    }

    public async Task<IReadOnlyList<Ticket>> GetSoldByBookingIdAsync(long bookingId, CancellationToken cancellationToken = default)
    {
        var daos = await context.Tickets
            .AsTracking()
            .Where(t => t.BookingId == bookingId)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Ticket>>(daos);
    }

    public async Task DecrementOfferInventoryAsync(long ticketId, int quantity, CancellationToken cancellationToken = default)
    {
        var dao = await context.Tickets
            .AsTracking()
            .FirstOrDefaultAsync(t =>
                t.Id == ticketId
                && t.SeatInventory > 0
                && t.SeatNumber == null
                && t.PassengerFullName == null
                && t.PassengerEmail == null,
                cancellationToken);

        if (dao == null) throw new KeyNotFoundException("Ticket offer not found.");

        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (quantity > dao.SeatInventory) throw new ArgumentException("Not enough seats available.");

        dao.SeatInventory -= quantity; 
    }

    public async Task IncrementOfferInventoryAsync(int flightScheduleId, string fareClass, int quantity, CancellationToken cancellationToken = default)
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
                cancellationToken);

        if (dao == null)
            throw new InvalidOperationException("Offer row not found for restore inventory.");

        dao.SeatInventory += quantity;
    }

    public async Task<int> CountSoldSeatsForScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default)
    {
        var offersBookingId = await context.Bookings
            .AsNoTracking()
            .Where(b => b.ConfirmationCode == "OFFERS00")
            .Select(b => b.Id)
            .SingleAsync(cancellationToken);

        return await context.Tickets
            .AsNoTracking()
            .CountAsync(t =>
                t.FlightScheduleId == flightScheduleId &&
                t.BookingId != offersBookingId, cancellationToken);
    }

    public async Task<IReadOnlyList<Ticket>> GetOffersByScheduleIdAsync(int flightScheduleId, CancellationToken cancellationToken = default)
    {
        var offersBookingId = await context.Bookings
            .AsNoTracking()
            .Where(b => b.ConfirmationCode == "OFFERS00")
            .Select(b => b.Id)
            .SingleAsync(cancellationToken);

        var daos = await context.Tickets
            .AsNoTracking()
            .Where(t => t.FlightScheduleId == flightScheduleId && t.BookingId == offersBookingId)
            .OrderBy(t => t.TotalPrice)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<Ticket>>(daos);
    }


}
