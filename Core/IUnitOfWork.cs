using Core.Entities;

namespace Core;
public interface IUnitOfWork : IDisposable
{
    public Task<int> CompleteAsync();

    //IRepository<Product, int> productRepository { get; }
    //IRepository<ProductBrand, int> productBrandRepository { get; }
    //IRepository<ProductType, int> productTypeRepository { get; }

    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
}
