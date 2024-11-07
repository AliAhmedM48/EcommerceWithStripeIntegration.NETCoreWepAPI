using Core.Entities;
using Core.Specifications;

namespace Core;
public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> specifications);
    Task<TEntity> GetAsync(TKey id);
    Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> specifications);
    Task<int> GetCountWithSpecAsync(ISpecifications<TEntity, TKey> specifications);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
