using System.Linq.Expressions;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Domain.Contracts
{
    public interface ISpecifications<TEntity> where TEntity : BaseEntity
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
