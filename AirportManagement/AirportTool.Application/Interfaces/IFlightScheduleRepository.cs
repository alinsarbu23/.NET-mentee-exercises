using AirportTool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces
{
    public interface IFlightScheduleRepository:IRepository<FlightSchedule>
    {
        Task<IReadOnlyList<FlightSchedule>> GetUpcomingAsync(
            DateTime fromUtc,
            DateTime toUtc,
            CancellationToken cancellationToken=default);


        Task<FlightSchedule?> FindByFlightAndDepartureAsync(
            int flightId,
            DateTime scheduledDepartureUtc,
            CancellationToken cancellationToken = default);

        Task<bool> HasGateOverlapAsync(
            int gateId,
            DateTime departureUtc,
            DateTime arrivalUtc,
            CancellationToken cancellationToken = default);

        Task<Gate?> GetGateByCodeAsync(
            int airportId,
            string gateCode,
            CancellationToken cancellationToken = default);
    }
}
