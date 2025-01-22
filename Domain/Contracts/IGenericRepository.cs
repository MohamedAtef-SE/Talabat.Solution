namespace Talabat.Core.Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> specs);
        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity,TKey> specs);
        Task<bool> AddAsync(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
    }
}
