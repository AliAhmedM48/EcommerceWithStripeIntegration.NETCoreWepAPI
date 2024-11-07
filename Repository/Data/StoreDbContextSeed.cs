using Core.Entities.Order;
using Core.Entities.Products;
using System.Text.Json;

namespace Repository.Data;
public static class StoreDbContextSeed
{
    public static async Task SeedAsync(StoreDbContext _context)
    {

        if (_context.Brands.Count() == 0)
        {
            var brandsJson = File.ReadAllText(@"..\repository\data\dataseed\brands.json");
            var brandsList = JsonSerializer.Deserialize<List<ProductBrand>>(brandsJson);

            if (brandsList is not null && brandsList.Count > 0)
            {
                await _context.AddRangeAsync(brandsList);
                await _context.SaveChangesAsync();
            }
        }

        if (_context.Types.Count() == 0)
        {
            var typesJson = File.ReadAllText(@"..\repository\data\dataseed\types.json");
            var typeslist = JsonSerializer.Deserialize<List<ProductType>>(typesJson);

            if (typeslist is not null && typeslist.Count > 0)
            {
                await _context.AddRangeAsync(typeslist);
                await _context.SaveChangesAsync();
            }
        }
        if (_context.Products.Count() == 0)
        {
            var productsData = File.ReadAllText(@"..\Repository\Data\DataSeed\products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products is not null && products.Count > 0)
            {
                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();
            }
        }

        if (_context.DeliveryMethods.Count() == 0)
        {
            var deliveryMethodsJson = File.ReadAllText(@"..\repository\data\dataseed\delivery.json");
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsJson);

            if (deliveryMethods is not null && deliveryMethods.Count > 0)
            {
                await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                await _context.SaveChangesAsync();
            }
        }
    }
}
