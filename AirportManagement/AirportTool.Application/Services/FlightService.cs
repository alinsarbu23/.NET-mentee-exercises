using AirportTool.Application.DTOs.Flights;
using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Common;
using AirportTool.Infrastructure.DTOs.Flights;
using AutoMapper;

namespace AirportTool.Application.Services.Flights
{
    public class FlightService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<GetFlightByIdDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var flight = await unitOfWork.Flights.GetByIdAsync(id, cancellationToken);

            if(flight is null)
            {
                return null;
            }

            return mapper.Map<GetFlightByIdDto>(flight);
        }

        public async Task<PagedResultDto<FlightSearchResultDto>> SearchAsync(GetFlightSearchDto query, CancellationToken cancellationToken = default)
        {
            return await unitOfWork.FlightSchedules.SearchAsync(query, cancellationToken);
        }

        public async Task<int> CreateAsync(CreateFlightDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.OriginIata == dto.DestinationIata)
            {
                throw new ArgumentException("Origin and destination are the same !");
            }

            var flight = mapper.Map<Flight>(dto);
            flight.IsActive = true;

            await unitOfWork.Flights.AddAsync(flight, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return flight.Id;
        }

        public async Task UpdateAsync(int id, UpdateFlightDto dto, CancellationToken cancellationToken = default)
        {
            var flight = await unitOfWork.Flights.GetByIdAsync(id, cancellationToken);
            if (flight is null)
            {
                throw new KeyNotFoundException("Flight not found");
            }
                
            if (dto.OriginIata == dto.DestinationIata)
            {
                throw new ArgumentException("Origin and destination cannot be the same.");
            }


            mapper.Map(dto, flight);

            await unitOfWork.Flights.UpdateAsync(flight, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await unitOfWork.Flights.DeleteAsync(id, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
