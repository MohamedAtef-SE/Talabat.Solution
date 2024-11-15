using Microsoft.EntityFrameworkCore;
using Talabat.Core.Application.Entities.Products;
using Talabat.Core.Contracts;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            //// Before using Specification DP
            //if (typeof(TEntity).Name == typeof(Product).Name)
            //{ 
            //    return (IReadOnlyList<TEntity>) await _dbContext.Product.Include(P => P.Brand).Include(P => P.Category).ToListAsync();
            //}

            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> specs)
        {
            return await SpecificationBuilder<TEntity>.GetQuery(_dbContext.Set<TEntity>(),specs).ToListAsync();
        }


        public async Task<TEntity?> GetAsync(int id)
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

    }
}
