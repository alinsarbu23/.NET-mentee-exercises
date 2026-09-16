using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirportTool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirportDbContext context;

        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public ITicketRepository Tickets { get; }
        public IBookingRepository Bookings { get; }
        public IAircraftRepository Aircraft { get; }
        public IGateRepository Gates { get; }

        public UnitOfWork(
                AirportDbContext context,
                IFlightRepository flights,
                IFlightScheduleRepository schedules,
                ITicketRepository tickets,
                IBookingRepository bookings,
                IAircraftRepository aircraft,
                IGateRepository gates)
        {
            this.context = context;
            Flights = flights;
            FlightSchedules = schedules;
            Tickets = tickets;
            Bookings = bookings;
            Aircraft = aircraft;
            Gates = gates;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await context.SaveChangesAsync(ct);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return context.Database.BeginTransactionAsync(cancellationToken);
        }
            
    }
}
