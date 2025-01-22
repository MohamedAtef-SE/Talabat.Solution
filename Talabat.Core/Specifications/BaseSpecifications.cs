using System.Linq.Expressions;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities._Common;

namespace Talabat.Core.Application.Specifications
{
    public class BaseSpecifications<TEntity,TKey> : ISpecifications<TEntity,TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool PaginationApplied { get; set; } = false;

        public BaseSpecifications(Expression<Func<TEntity, bool>> criteria,string? sort = null)
        {
            Criteria = criteria;

            AddIncludes();
            SortedBy(sort);
        }

        protected virtual void AddIncludes()
        {

        }

        protected virtual void SortedBy(string? sort)
        {

        }
    }
}
