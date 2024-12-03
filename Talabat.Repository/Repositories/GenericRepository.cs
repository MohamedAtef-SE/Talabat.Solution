using Microsoft.EntityFrameworkCore;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities._Common;
using Talabat.Infrastructure.Persistence.Data;

namespace Talabat.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity,TKey> : IGenericRepository<TEntity,TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        private readonly StoreContext _dbContext;
       
        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> specs)
        {
            return await SpecificationBuilder<TEntity,TKey>.GetQuery(_dbContext.Set<TEntity>(),specs).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity,TKey> specs)
        {
            return await SpecificationBuilder<TEntity,TKey>.GetQuery(_dbContext.Set<TEntity>(),specs).FirstOrDefaultAsync();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            var result = await _dbContext.Set<TEntity>().AddAsync(entity);

            return result.State is EntityState.Added;
               
        }

        public bool Update(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Update(entity);
            return result.State is EntityState.Modified;
               
        }

        public bool Delete(TEntity entity)
        {
            var result = _dbContext.Set<TEntity>().Remove(entity);
            return result.State is EntityState.Deleted;
        }

    }
}
