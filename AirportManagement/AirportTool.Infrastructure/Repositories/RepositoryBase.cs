using AirportTool.Application.Interfaces;
using AirportTool.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class RepositoryBase<TDomain, TDao, TKey> : IRepository<TDomain, TKey>
        where TDomain : class
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

        public virtual async Task<TDomain?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var dao = await dbSet.FindAsync(new object[] { id! }, cancellationToken);
            return dao == null ? null : mapper.Map<TDomain>(dao);
        }

        public virtual async Task<IReadOnlyList<TDomain>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var daos = await dbSet.AsNoTracking().ToListAsync(cancellationToken);
            return mapper.Map<List<TDomain>>(daos);
        }

        public virtual async Task AddAsync(TDomain entity, CancellationToken cancellationToken = default)
        {
            var dao = mapper.Map<TDao>(entity);
            await dbSet.AddAsync(dao, cancellationToken);
        }

        public virtual Task UpdateAsync(TDomain entity, CancellationToken cancellationToken = default)
        {
            var dao = mapper.Map<TDao>(entity);
            dbSet.Update(dao);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var dao = await dbSet.FindAsync(new object[] { id! }, cancellationToken);
            if (dao != null)
            {
                dbSet.Remove(dao);
            }
        }
    }
}
