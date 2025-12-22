using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;

public interface ITicketRepository : IRepository<Ticket, long>
{
    Task<Ticket?> GetOfferByIdAsync(long ticketId, CancellationToken cancellationToken = default);

    Task DecrementOfferInventoryAsync(long ticketId, int quantity, CancellationToken cancellationToken = default);
    Task IncrementOfferInventoryAsync(int flightScheduleId, string fareClass, int quantity, CancellationToken cancellationToken = default);

    Task<int> CountSoldSeatsForScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Ticket>> GetSoldByBookingIdAsync(long bookingId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Ticket>> GetByFlightScheduleIdAsync(int flightScheduleId, CancellationToken cancellationToken = default);
}
