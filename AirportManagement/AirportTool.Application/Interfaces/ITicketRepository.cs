using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IReadOnlyList<Ticket>> GetByFlightScheduleIdAsync(
            int flightScheduleId,
            CancellationToken ct = default);
    }
}

