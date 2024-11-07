namespace Core.Dtos.Orders;

public class OrderReturnedDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public string Status { get; set; }
    public ShippingAddressDto ShippingAddressDto { get; set; }
    public string DeliveryMethodName { get; set; }
    public decimal DeliveryMethodCost { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
    public string? PaymentIntentId { get; set; } = string.Empty;
}