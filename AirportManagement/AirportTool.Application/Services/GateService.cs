using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.DTOs.Gates;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class GateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<GetGateByIdDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await unitOfWork.Gates.GetByIdAsync(id, ct);
            return entity == null ? null : mapper.Map<GetGateByIdDto>(entity);
        }

        public async Task<int> CreateAsync(CreateGateDto dto, CancellationToken ct = default)
        {
            if (await unitOfWork.Gates.GateCodeExistsAsync(dto.AirportId, dto.Code, null, ct))
                throw new ArgumentException("Gate code already exists for this airport.");

            var entity = mapper.Map<Gate>(dto);

            await unitOfWork.Gates.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return entity.Id;
        }

        public async Task UpdateAsync(int id, UpdateGateDto dto, CancellationToken cancellationToken = default)
        {
            var gate = await unitOfWork.Gates.GetByIdAsync(id, cancellationToken);
            if (gate == null)
            {
                throw new KeyNotFoundException("Gate not found.");
            }

            if (await unitOfWork.Gates.GateCodeExistsAsync(gate.AirportId, dto.Code, id, cancellationToken))
            {
                throw new ArgumentException("Gate code already exists for this airport.");
            }
                
            gate.Code = dto.Code.Trim();

            await unitOfWork.Gates.UpdateAsync(gate, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            if (await unitOfWork.Gates.IsUsedInSchedulesAsync(id, ct))
                throw new InvalidOperationException("Cannot delete gate because it is used by flight schedules.");

            await unitOfWork.Gates.DeleteAsync(id, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
