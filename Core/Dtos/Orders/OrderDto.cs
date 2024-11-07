namespace Core.Dtos.Orders;
public class OrderDto
{
    public string CartId { get; set; }
    public int DeliveryMethodId { get; set; }
    public ShippingAddressDto ShippingAddressDto { get; set; }

}
