namespace Core.Entities.Order;
public class OrderItem : BaseEntity<int>
{
    public OrderItem()
    {

    }
    public OrderItem(ProductItemOrder product, decimal price, int quantity)
    {
        Product = product;
        Price = price;
        Quantity = quantity;
    }

    public ProductItemOrder Product { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
}
