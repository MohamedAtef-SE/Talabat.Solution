using System.Linq.Expressions;

namespace Talabat.Core.Domain.Contracts
{
    public interface ISpecifications<TEntity,TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool PaginationApplied { get; set; }

    }
}
