namespace Core.Entities.Carts;
public class Cart
{
    public string Id { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
    public int? DeliveryMethodId { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
}
