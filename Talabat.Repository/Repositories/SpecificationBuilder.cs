using Microsoft.EntityFrameworkCore;
using Talabat.Core.Application.Entities.Products;
using Talabat.Core.Contracts;

namespace Talabat.Repository.Repositories
{
    internal static class SpecificationBuilder<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> DbSet,ISpecifications<TEntity> specs)
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
