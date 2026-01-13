using AirportTool.Application.DTOs.Schedules;
using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Schedules;
using AutoMapper;
using System.Text.Json;

namespace AirportTool.Application.Services.Schedules
{
    public class ScheduleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<GetScheduleByIdDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var schedule = await unitOfWork.FlightSchedules.GetByIdAsync(id, cancellationToken);

            if (schedule is null)
            {
                return null;
            }

            return mapper.Map<GetScheduleByIdDto>(schedule);
        }

        public async Task<int> CreateAsync(CreateScheduleDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.ScheduledArrivalUtc <= dto.ScheduledDepartureUtc)
            {
                throw new ArgumentException("Arrival time must be after departure time.");
            }

            if (dto.GateId.HasValue)
            {
                var overlap = await unitOfWork.FlightSchedules.HasGateOverlapAsync(
                    dto.GateId.Value,
                    dto.ScheduledDepartureUtc,
                    dto.ScheduledArrivalUtc,
                    null,
                    cancellationToken);

                if (overlap)
                {
                    throw new InvalidOperationException("Gate overlap detected for the selected gate and time window.");
                }
            }

            var schedule = mapper.Map<FlightSchedule>(dto);
            schedule.FlightStatusId = dto.FlightStatusId; 

            await unitOfWork.FlightSchedules.AddAsync(schedule, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return schedule.Id;
        }

        public async Task<IReadOnlyList<ScheduleStatsDto>> GetUpcomingStatsAsync(CancellationToken cancellationToken = default)
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime nextWeek = today.AddDays(7);

            var schedules = await unitOfWork.FlightSchedules.GetUpcomingAsync(
                today,
                nextWeek,
                cancellationToken);

            var stats = schedules
                .GroupBy(s => s.ScheduledDepartureUtc.Date)
                .Select(g => new ScheduleStatsDto
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    FlightsCount = g.Count()
                })
                .OrderBy(s => s.Date)
                .ToList();

            return stats;
        }

        public async Task<IReadOnlyList<GetScheduleByIdDto>> SearchAsync(
            int? flightId = null,
            DateTime? fromUtc = null,
            DateTime? toUtc = null,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<FlightSchedule> domainList;

            if (fromUtc.HasValue && toUtc.HasValue)
            {
                var list = await unitOfWork.FlightSchedules.GetUpcomingAsync(fromUtc.Value, toUtc.Value, cancellationToken);
                domainList = list;
            }
            else
            {
                var all = await unitOfWork.FlightSchedules.GetAllAsync(cancellationToken);
                domainList = all;
            }

            if (flightId.HasValue)
            {
                domainList = domainList.Where(s => s.FlightId == flightId.Value);
            }

            var dtos = mapper.Map<IReadOnlyList<GetScheduleByIdDto>>(domainList.ToList());
            return dtos;
        }

        public async Task<ImportScheduleResultDto> ImportAsync(Stream jsonStream, CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(jsonStream);
            var json = await reader.ReadToEndAsync(cancellationToken);

            var rows = JsonSerializer.Deserialize<List<ImportScheduleRowDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (rows == null || rows.Count == 0)
            {
                throw new ArgumentException("Empty or invalid JSON.");
            }
                

            if (rows.Count > 1000)
            {
                throw new InvalidOperationException("Maximum 1000 rows.");
            }
               
            var result = new ImportScheduleResultDto
            {
                Total = rows.Count
            };

            for (int i = 0; i < rows.Count; i++)
            {
                var rowIndex = i + 1;
                var row = rows[i];

                try
                {
                    var outcome = await ImportSingleRowAsync(row, cancellationToken);

                    if (outcome == ImportOutcome.Created) result.Created++;
                    else if (outcome == ImportOutcome.Updated) result.Updated++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add(new ImportScheduleErrorDto
                    {
                        Row = rowIndex,
                        Message = ex.Message
                    });
                }
            }

            return result;
        }

        private enum ImportOutcome { Created, Updated }

        private async Task<ImportOutcome> ImportSingleRowAsync(ImportScheduleRowDto row, CancellationToken cancellationToken)
        {
            if (row.ScheduledArrivalUtc <= row.ScheduledDepartureUtc)
            {
                throw new ArgumentException("Arrival must be after departure.");
            }


            var airline = await unitOfWork.Flights.GetAirlineByIataAsync(row.AirlineIata, cancellationToken);
            if (airline == null)
            {
                throw new InvalidOperationException($"Airline {row.AirlineIata} not found.");
            }

            var origin = await unitOfWork.Flights.GetAirportByIataAsync(row.OriginIata, cancellationToken);
            if(origin == null)
            {
                throw new InvalidOperationException($"Origin airport {row.OriginIata} not found.");
            }

            var destination = await unitOfWork.Flights.GetAirportByIataAsync(row.DestinationIata, cancellationToken);
            if(destination == null)
            {
                throw new InvalidOperationException($"Destination airport {row.DestinationIata} not found.");
            }
                              
            var flight = await unitOfWork.Flights.GetByKeyAsync(
                airline.Id,
                row.FlightNumber,
                origin.Id,
                destination.Id,
                cancellationToken);

            if (flight == null)
            {
                flight = new Flight
                {
                    AirlineId = airline.Id,
                    FlightNumber = row.FlightNumber,
                    OriginAirportId = origin.Id,
                    DestinationAirportId = destination.Id,
                    IsActive = true
                };

                await unitOfWork.Flights.AddAsync(flight, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken); 
            }

            var schedule = await unitOfWork.FlightSchedules.FindByFlightAndDepartureAsync(
                flight.Id,
                row.ScheduledDepartureUtc,
                cancellationToken);

            int? gateId = null;

            if (!string.IsNullOrWhiteSpace(row.GateCode))
            {
                var gate = await unitOfWork.FlightSchedules.GetGateByCodeAsync(
                    origin.Id,
                    row.GateCode!,
                    cancellationToken);

                if (gate is null)
                {
                    throw new InvalidOperationException($"Gate {row.GateCode} not found.");
                }
                    

                var overlap = await unitOfWork.FlightSchedules.HasGateOverlapAsync(
                    gateId: gate.Id,
                    departureUtc: row.ScheduledDepartureUtc,
                    arrivalUtc: row.ScheduledArrivalUtc,
                    excludeScheduleId: schedule?.Id,              
                    cancellationToken: cancellationToken);

                if (overlap)
                {
                    throw new InvalidOperationException("Gate overlap detected.");
                }
                    

                gateId = gate.Id;
            }

            if (schedule == null)
            {
                schedule = new FlightSchedule
                {
                    FlightId = flight.Id,
                    ScheduledDepartureUtc = row.ScheduledDepartureUtc,
                    ScheduledArrivalUtc = row.ScheduledArrivalUtc,
                    GateId = gateId,
                    FlightStatusId = 1
                };

                await unitOfWork.FlightSchedules.AddAsync(schedule, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return ImportOutcome.Created;
            }
            else
            {
                schedule.ScheduledArrivalUtc = row.ScheduledArrivalUtc;
                schedule.GateId = gateId;

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return ImportOutcome.Updated;
            }
        }


        public async Task UpdateAsync(int id, UpdateScheduleDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.ScheduledArrivalUtc <= dto.ScheduledDepartureUtc)
            {
                throw new ArgumentException("ScheduledArrivalUtc must be after ScheduledDepartureUtc.");
            }
                
            var schedule = await unitOfWork.FlightSchedules.GetByIdAsync(id, cancellationToken);
            if (schedule == null)
            {
                throw new KeyNotFoundException("Schedule not found.");
            }

            if (dto.GateId.HasValue)
            {
                var overlap = await unitOfWork.FlightSchedules.HasGateOverlapAsync(
                    dto.GateId.Value,
                    dto.ScheduledDepartureUtc,
                    dto.ScheduledArrivalUtc,
                    excludeScheduleId: id,     
                    cancellationToken: cancellationToken);

                if (overlap)
                {
                    throw new InvalidOperationException("Gate overlap detected for the given time window.");
                }
                    
            }

            schedule.ScheduledDepartureUtc = dto.ScheduledDepartureUtc;
            schedule.ScheduledArrivalUtc = dto.ScheduledArrivalUtc;
            schedule.GateId = dto.GateId;
            schedule.FlightStatusId = dto.FlightStatusId;

            await unitOfWork.FlightSchedules.UpdateAsync(schedule, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
