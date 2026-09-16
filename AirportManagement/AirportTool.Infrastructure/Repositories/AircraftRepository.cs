using AirportTool.Application.Interfaces;
using AirportTool.Domain.Models;
using AirportTool.Infrastructure.Data;
using AirportTool.Infrastructure.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class AircraftRepository : RepositoryBase<Aircraft, AircraftDAO, int>, IAircraftRepository
    {
        public AircraftRepository(AirportDbContext context, IMapper mapper)
            : base(context, mapper, context.Aircraft) { }

        public async Task<bool> TailNumberExistsAsync(string tailNumber, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            tailNumber = tailNumber.Trim();

            return await context.Aircraft.AsNoTracking().AnyAsync(aircraft => aircraft.TailNumber == tailNumber &&
                    (excludeId == null || aircraft.Id != excludeId.Value), cancellationToken);
        }
    }
}
