using Core;
using Core.Entities;
using Core.Entities.Products;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace Repository;
public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly StoreDbContext context;

    public Repository(StoreDbContext context)
    {
        this.context = context;
    }

    // ------------------------------------------------------------

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        if (typeof(TEntity) == typeof(Product))
        {
            return (IEnumerable<TEntity>)await context.Products.Include(p => p.Brand).Include(p => p.Type).ToListAsync();
        }
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetAsync(TKey id)
    {

        if (typeof(TEntity) == typeof(Product))
        {
            return await context.Products.Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity;
        }
        return await context.Set<TEntity>().FindAsync(id);
    }

    // ------------------------------------------------------------

    public async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
    }
    public void Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
    {
        return await ApplySpecifications(specifications).ToListAsync();
    }

    public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
    {
        return await ApplySpecifications(specifications).FirstOrDefaultAsync();
    }

    public async Task<int> GetCountWithSpecAsync(ISpecifications<TEntity, TKey> specifications)
    {
        return await ApplySpecifications(specifications).CountAsync();
    }

    private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> specifications)
    {
        return SpecificationsEvaluator<TEntity, TKey>.GetQuery(context.Set<TEntity>(), specifications);
    }


}
