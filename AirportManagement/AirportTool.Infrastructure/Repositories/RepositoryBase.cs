using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class RepositoryBase<TDomain, TDao> : IRepository<TDomain> where TDomain : class 
        where TDao : class
    {
        protected readonly AirportDbContext context;
        protected readonly IMapper mapper;
        protected readonly DbSet<TDao> dbSet;

        public RepositoryBase(AirportDbContext context, IMapper mapper, DbSet<TDao> dbSet)
        {
            this.context = context;
            this.mapper = mapper;
            this.dbSet = dbSet;
        }

        public virtual async Task<TDomain?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var dao = await dbSet.FindAsync(new object[] { id }, cancellationToken);
            if(dao is null)
            {
                return null;
            }
            return mapper.Map<TDomain>(dao);
        }

        public virtual async Task<IReadOnlyList<TDomain>> GetAllAsync(CancellationToken ct = default)
        {
            var daos = await dbSet.ToListAsync(ct);
            return mapper.Map<List<TDomain>>(daos);
        }

        public virtual async Task AddAsync(TDomain entity, CancellationToken ct = default)
        {
            var dao = mapper.Map<TDao>(entity);
            await dbSet.AddAsync(dao, ct);
        }

        public virtual Task UpdateAsync(TDomain entity, CancellationToken ct = default)
        {
            var dao = mapper.Map<TDao>(entity);
            dbSet.Update(dao);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var dao = await dbSet.FindAsync(new object[] { id }, ct);
            if (dao != null)
            {
                dbSet.Remove(dao);
            }
                
        }
    }
}

