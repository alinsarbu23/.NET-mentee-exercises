using AirportTool.Domain.Models;

namespace AirportTool.Application.Interfaces
{
    public interface IAircraftRepository : IRepository<Aircraft, int>
    {
        Task<bool> TailNumberExistsAsync(string tailNumber, int? excludeId = null, CancellationToken cancellationToken = default);
    }
}
