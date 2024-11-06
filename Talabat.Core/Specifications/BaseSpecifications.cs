using System.Linq.Expressions;
using Talabat.Core.Contracts;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<BaseEntity> : ISpecifications<BaseEntity>
    {
        public Expression<Func<BaseEntity, bool>> Criteria { get; }
        public List<Expression<Func<BaseEntity, object>>> Includes { get; } = new List<Expression<Func<BaseEntity, object>>>();

        public BaseSpecifications(Expression<Func<BaseEntity, bool>> criteria)
        {
            Criteria = criteria;

            AddIncludes();
        }

        protected virtual void AddIncludes()
        {

        }

    }
}
