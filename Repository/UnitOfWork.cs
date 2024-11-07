using Core;
using Core.Entities;
using Repository.Data;
using System.Collections;

namespace Repository;
public class UnitOfWork : IUnitOfWork
{

    private readonly Hashtable _repositories;
    private readonly StoreDbContext context;
    //public IRepository<Product, int> productRepository { get; }
    //public IRepository<ProductBrand, int> productBrandRepository { get; }
    //public IRepository<ProductType, int> productTypeRepository { get; }

    public UnitOfWork(StoreDbContext context)
    {
        this.context = context;
        this._repositories = new Hashtable();
        //productRepository = new Repository<Product, int>(this.context);
        //productBrandRepository = new Repository<ProductBrand, int>(this.context);
        //productTypeRepository = new Repository<ProductType, int>(this.context);
    }



    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        Console.WriteLine("Dispose StoreDbContext");
    }

    public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type))
        {
            Console.WriteLine($"NEWWWWWW => repository {type}");
            var repository = new Repository<TEntity, TKey>(context);
            _repositories.Add(type, repository);
        }
        return _repositories[type] as IRepository<TEntity, TKey>;
    }
}
