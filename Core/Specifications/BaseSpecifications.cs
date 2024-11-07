using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;
public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public Expression<Func<TEntity, bool>> Criteria { get; set; } = null;
    public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
    public Expression<Func<TEntity, object>> OrderBy { get; set; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginationEnabled { get; set; }

    public BaseSpecifications(Expression<Func<TEntity, bool>> expression)
    {
        Criteria = expression;
    }

    public BaseSpecifications()
    {
    }

    public void AddOrderBy(Expression<Func<TEntity, object>> expression)
    {
        OrderBy = expression;
    }

    public void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
    {
        OrderByDescending = expression;
    }
    public void ApplyPagination(int skip, int take)
    {
        IsPaginationEnabled = true;
        Skip = skip;
        Take = take;
    }
}
