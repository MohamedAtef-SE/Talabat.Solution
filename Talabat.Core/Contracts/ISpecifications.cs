using System.Linq.Expressions;
using Talabat.Core.Application.Entities.Products;

namespace Talabat.Core.Contracts
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
