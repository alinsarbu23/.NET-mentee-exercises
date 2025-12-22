using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IFlightRepository Flights { get; }

        IFlightScheduleRepository FlightSchedules { get; }

        ITicketRepository Tickets { get; }

        IBookingRepository Bookings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    }
}
