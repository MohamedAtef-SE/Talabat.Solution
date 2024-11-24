using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Domain.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> specs);
        Task<TEntity?> GetAsync(string id);
        Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity> specs);
        Task<bool> AddAsync(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
    }
}
