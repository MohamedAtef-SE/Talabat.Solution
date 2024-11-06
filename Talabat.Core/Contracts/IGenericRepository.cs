using Talabat.Core.Entities;

namespace Talabat.Core.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> specs);
        Task<TEntity?> GetAsync(int id);
        Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity> specs);

    }
}
