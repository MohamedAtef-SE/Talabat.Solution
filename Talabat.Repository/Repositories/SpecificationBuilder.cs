using Microsoft.EntityFrameworkCore;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities._Common;

namespace Talabat.Infrastructure.Persistence.Repositories
{
    internal static class SpecificationBuilder<TEntity,TKey> where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> DbSet,ISpecifications<TEntity,TKey> specs)
        {
            var query = DbSet;
            
            if(specs is null) return query;

            if (specs.Criteria is not null)
            {
                query =query.Where(specs.Criteria);
            }


            if (specs.OrderBy is not null)
            {
                query = query.OrderBy(specs.OrderBy);
            }
            else if (specs.OrderByDesc is not null)
            {
                query = query.OrderByDescending(specs.OrderByDesc);
            }

            if (specs.PaginationApplied)
            {
                query = query.Skip(specs.Skip).Take(specs.Take);
            }

            if (specs.Includes?.Count > 0)
            {
               query = specs.Includes.Aggregate(query,(currentQuery,includeExpression) => currentQuery.Include(includeExpression) );
            }


            return query;
        }
    }
}
