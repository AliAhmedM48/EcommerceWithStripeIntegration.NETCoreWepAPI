namespace Core.Dtos.Carts;
public class CartDto
{
    public string Id { get; set; }
    public IEnumerable<CartItemDto> Items { get; set; }
    public int? DeliveryMethodId { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
}
