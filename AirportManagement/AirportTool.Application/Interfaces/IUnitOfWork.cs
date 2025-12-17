using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AirportTool.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IFlightRepository Flights { get; }

        IFlightScheduleRepository FlightSchedules { get; }

        ITicketRepository Tickets { get; }

        IBookingRepository Bookings { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
