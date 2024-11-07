using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public static class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
    {
        var query = inputQuery.AsQueryable();

        if (spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria);
        }

        // Sorting
        if (spec.OrderBy is not null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }


        if (spec.IsPaginationEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        // for loop
        //query.Include(spec.Includes[0]);
        query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

        return query;
    }
}
