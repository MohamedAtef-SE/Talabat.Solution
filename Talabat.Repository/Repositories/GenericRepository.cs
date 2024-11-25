using Microsoft.EntityFrameworkCore;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence.Data;

namespace Talabat.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
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

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> specs)
        {
            return await SpecificationBuilder<TEntity>.GetQuery(_dbContext.Set<TEntity>(),specs).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            //// Before using Specification DP
            //if (typeof(TEntity).Name == typeof(Product).Name)
            //{
            //    return  await _dbContext.Product.Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync(P => P.Id.Equals(id)) as TEntity;
            //}
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity> specs)
        {
            return await SpecificationBuilder<TEntity>.GetQuery(_dbContext.Set<TEntity>(),specs).FirstOrDefaultAsync();
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
