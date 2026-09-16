using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class GateRepository : RepositoryBase<Gate, GateDAO, int>, IGateRepository
    {
        public GateRepository(AirportDbContext context, IMapper mapper)
            : base(context, mapper, context.Gates)
        {
        }

        public async Task<bool> GateCodeExistsAsync(
            int airportId,
            string code,
            int? excludeGateId = null,
            CancellationToken ct = default)
        {
            var normalized = code.Trim();

            return await context.Gates
                .AsNoTracking()
                .AnyAsync(g =>
                    g.AirportId == airportId &&
                    g.Code == normalized &&
                    (excludeGateId == null || g.Id != excludeGateId.Value),
                    ct);
        }

        public async Task<bool> IsUsedInSchedulesAsync(int gateId, CancellationToken ct = default)
        {
            return await context.FlightSchedules
                .AsNoTracking()
                .AnyAsync(fs => fs.GateId == gateId, ct);
        }
    }
}
