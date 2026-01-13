using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Aircraft;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class AircraftService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<GetAircraftByIdDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await unitOfWork.Aircraft.GetByIdAsync(id, ct);
            return entity == null ? null : mapper.Map<GetAircraftByIdDto>(entity);
        }

        public async Task<int> CreateAsync(CreateAircraftDto dto, CancellationToken ct = default)
        {
            if (dto.SeatCapacity <= 0) throw new ArgumentException("SeatCapacity must be > 0.");

            if (await unitOfWork.Aircraft.TailNumberExistsAsync(dto.TailNumber, null, ct))
                throw new ArgumentException("TailNumber already exists.");

            var entity = mapper.Map<Aircraft>(dto);

            await unitOfWork.Aircraft.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateAircraftDto dto, CancellationToken ct = default)
        {
            if (dto.SeatCapacity <= 0) throw new ArgumentException("SeatCapacity must be > 0.");

            var entity = await unitOfWork.Aircraft.GetByIdAsync(id, ct);
            if (entity == null) throw new KeyNotFoundException("Aircraft not found.");

            if (await unitOfWork.Aircraft.TailNumberExistsAsync(dto.TailNumber, id, ct))
                throw new ArgumentException("TailNumber already exists.");

            mapper.Map(dto, entity);

            await unitOfWork.Aircraft.UpdateAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await unitOfWork.Aircraft.DeleteAsync(id, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
