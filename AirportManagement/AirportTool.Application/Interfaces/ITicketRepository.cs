using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket, long>
    {
        Task<IReadOnlyList<Ticket>> GetByFlightScheduleIdAsync(
            int flightScheduleId,
            CancellationToken cancellationToken = default);
    }
}

