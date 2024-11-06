using Microsoft.EntityFrameworkCore;
using Talabat.Core.Contracts;
using Talabat.Core.Entities;
using Talabat.Repository.Data;

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

            if (specs.Includes?.Count > 0)
            {
               query = specs.Includes.Aggregate(query,(currentQuery,includeExpression) => currentQuery.Include(includeExpression) );
            }


            return query;
        }
    }
}
