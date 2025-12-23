using AirportTool.Application.DTOs.Flights;
using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data.Models;
using AirportTool.Infrastructure.DTOs.Common;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightScheduleRepository: RepositoryBase<FlightSchedule, FlightScheduleDAO, int>, IFlightScheduleRepository
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

        public async Task<bool> HasGateOverlapAsync(
            int gateId,
            DateTime departureUtc,
            DateTime arrivalUtc,
            CancellationToken cancellationToken = default)
        {
            var exists = await context.FlightSchedules.AnyAsync(
                schedule =>
                    schedule.GateId == gateId &&
                    schedule.ScheduledDepartureUtc < arrivalUtc &&
                    schedule.ScheduledArrivalUtc > departureUtc,
                cancellationToken);

            return exists;
        }

        public async Task<Gate?> GetGateByCodeAsync(int airportId,string gateCode, CancellationToken cancellationToken = default)
        {
            var dao = await context.Gates.FirstOrDefaultAsync(g =>
                g.AirportId == airportId &&
                g.Code == gateCode,
                cancellationToken);

            return dao == null ? null : mapper.Map<Gate>(dao);
        }


        public async Task<int> GetCapacityForScheduleAsync(int flightScheduleId, CancellationToken ct = default)
        {
            var schedule = await context.FlightSchedules
                .AsNoTracking()
                .Include(fs => fs.AssignedAircraft)
                .Include(fs => fs.Flight)
                    .ThenInclude(f => f.DefaultAircraft)
                .FirstOrDefaultAsync(fs => fs.Id == flightScheduleId, ct);

            if (schedule == null)
            {
                throw new KeyNotFoundException("Flight schedule not found.");
                }

            var cap = schedule.AssignedAircraft?.SeatCapacity
                      ?? schedule.Flight?.DefaultAircraft?.SeatCapacity;

            if (cap == null)
            {
                throw new InvalidOperationException("Cannot determine aircraft capacity.");
            }
            return cap.Value;
        }

        public async Task<PagedResultDto<FlightSearchResultDto>> SearchAsync(GetFlightSearchDto query, CancellationToken cancellationToken = default)
        {
            var pageNumber = query.Page;

            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            var pageSize = query.PageSize;
            if (pageSize <= 0)
            {
                pageSize = 20;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var results = context.FlightSchedules.AsNoTracking()
                .Include(fs => fs.Flight).ThenInclude(f => f.Airline)
                .Include(fs => fs.Flight).ThenInclude(f => f.OriginAirport)
                .Include(fs => fs.Flight).ThenInclude(f => f.DestinationAirport)
                .Include(fs => fs.Gate)
                .Include(fs => fs.FlightStatus)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.OriginIata))
            {
                results = results.Where(fs => fs.Flight.OriginAirport.IATACode == query.OriginIata);
            }

            if (!string.IsNullOrEmpty(query.DestinationIata))
            {
                results = results.Where(fs => fs.Flight.DestinationAirport.IATACode == query.DestinationIata);
            }

            if (!string.IsNullOrEmpty(query.AirlineIata))
            {
                results = results.Where(fs => fs.Flight.Airline.IATACode == query.AirlineIata);
            }

            if (query.Date.HasValue)
            {
                var start = query.Date.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                var end = start.AddDays(1);
                results = results.Where(s => s.ScheduledDepartureUtc >= start && s.ScheduledDepartureUtc < end);
            }
            else
            {
                results = results.Where(s => s.ScheduledDepartureUtc >= DateTime.UtcNow);
            }

            var totalCount = await results.CountAsync(cancellationToken);

            var items = await results.OrderBy(s => s.ScheduledDepartureUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(flightSchedule => new FlightSearchResultDto
                {
                    ScheduleId = flightSchedule.Id,
                    AirlineIata = flightSchedule.Flight.Airline.IATACode,
                    FlightNumber = flightSchedule.Flight.FlightNumber,
                    OriginIata = flightSchedule.Flight.OriginAirport.IATACode,
                    DestinationIata = flightSchedule.Flight.DestinationAirport.IATACode,
                    ScheduledDepartureUtc = flightSchedule.ScheduledDepartureUtc,
                    ScheduledArrivalUtc = flightSchedule.ScheduledArrivalUtc,
                    GateCode = flightSchedule.Gate != null ? flightSchedule.Gate.Code : null,
                    Status = flightSchedule.FlightStatus.Status
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<FlightSearchResultDto>
            {
                Items = items,
                Page = pageNumber,
                PageSize = pageSize,
                Total = totalCount
            };
        }
    }
}
