using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Data;

namespace AirportTool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirportDbContext context;

        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public ITicketRepository Tickets { get; }
        public IBookingRepository Bookings { get; }

        public UnitOfWork(
                AirportDbContext context,
                IFlightRepository flights,
                IFlightScheduleRepository schedules,
                ITicketRepository tickets,
                IBookingRepository bookings)
        {
            this.context = context;
            Flights = flights;
            FlightSchedules = schedules;
            Tickets = tickets;
            Bookings = bookings;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await context.SaveChangesAsync(ct);
        }
    }
}
