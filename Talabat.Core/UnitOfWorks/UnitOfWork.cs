using System.Collections.Concurrent;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities._Common;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Repositories;

namespace Talabat.Core.Application.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public ConcurrentDictionary<string, object> _repositories { get; set; }

        private readonly StoreContext _dbContext;
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new ConcurrentDictionary<string, object>();
        }
        public IGenericRepository<TEntity,TKey> GetRepository<TEntity,TKey>()
            where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
        {
            return (IGenericRepository<TEntity,TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity,TKey>(_dbContext));
        }

        public async Task<bool> CompleteAsync() => await _dbContext.SaveChangesAsync() > 0;

        public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

    }
}
