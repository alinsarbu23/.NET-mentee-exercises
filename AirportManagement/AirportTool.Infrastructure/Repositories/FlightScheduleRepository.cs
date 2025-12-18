using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightScheduleRepository: RepositoryBase<FlightSchedule, FlightScheduleDAO>, IFlightScheduleRepository
    {
        public FlightScheduleRepository(AirportDbContext context, IMapper mapper): base(context, mapper, context.FlightSchedules)
        {
        }

        public async Task<IReadOnlyList<FlightSchedule>> GetUpcomingAsync(DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            var daos = await context.FlightSchedules.Where(schedule => schedule.ScheduledDepartureUtc >= fromUtc && schedule.ScheduledDepartureUtc <= toUtc).ToListAsync(ct);

            return mapper.Map<List<FlightSchedule>>(daos);
        }

        public async Task<FlightSchedule?> FindByFlightAndDepartureAsync(int flightId, DateTime departureUtc, CancellationToken ct = default)
        {
            var dao = await context.FlightSchedules.FirstOrDefaultAsync(schedule => schedule.FlightId == flightId && schedule.ScheduledDepartureUtc == departureUtc, ct);

            if(dao is null)
            {
                return null;
            }

            return mapper.Map<FlightSchedule>(dao);
        }
    }
}
