using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class BookingRepository
        : RepositoryBase<Booking, BookingDAO, long>, IBookingRepository
    {
        public BookingRepository(AirportDbContext context, IMapper mapper): base(context, mapper, context.Bookings)
        {

        }

        public async Task<Booking?> GetConfirmationCodeAsync(string confirmationCode, CancellationToken cancellationToken = default)
        {
            var dao = await context.Bookings.Include(b => b.Tickets).Include(b => b.BookingStatus).FirstOrDefaultAsync(b => b.ConfirmationCode == confirmationCode, cancellationToken);

            if(dao is null)
            {
                return null;
            }

            return mapper.Map<Booking>(dao);
        }

        public async Task<IReadOnlyList<Booking>> GetActiveByFlightScheduleAsync(int flightScheduleId, CancellationToken cancellationToken = default)
        {
            var daos = await context.Bookings.Include(booking => booking.Tickets)
                .Where(booking => booking.Tickets.Any(ticket => ticket.FlightScheduleId == flightScheduleId)
                        && booking.BookingStatus.Status == "Active").ToListAsync(cancellationToken);

            return mapper.Map<List<Booking>>(daos);
        }



        public async Task SetStatusByCodeAsync(string confirmationCode, int bookingStatusId, CancellationToken ct = default)
        {
            var dao = await context.Bookings
                .AsTracking()
                .FirstOrDefaultAsync(b => b.ConfirmationCode == confirmationCode, ct);

            if (dao == null) throw new KeyNotFoundException("Booking not found.");

            dao.BookingStatusId = bookingStatusId;
        }
    }
}
