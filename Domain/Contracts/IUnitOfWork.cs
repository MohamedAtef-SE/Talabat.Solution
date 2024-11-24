using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Domain.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task<bool> CompleteAsync();
    }
}
