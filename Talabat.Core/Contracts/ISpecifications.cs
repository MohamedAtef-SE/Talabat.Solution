using System.Linq.Expressions;

namespace Talabat.Core.Contracts
{
    public interface ISpecifications<TEntity>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; }

    }
}
