using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface IGateRepository : IRepository<Gate, int>
    {
        Task<bool> GateCodeExistsAsync(
            int airportId,
            string code,
            int? excludeGateId = null,
            CancellationToken ct = default);

        Task<bool> IsUsedInSchedulesAsync(
            int gateId,
            CancellationToken ct = default);
    }
}
