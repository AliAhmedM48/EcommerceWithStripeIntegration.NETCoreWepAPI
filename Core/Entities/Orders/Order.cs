namespace Core.Entities.Order;

public class Order : BaseEntity<int>
{
    public Order()
    {

    }
    public Order(string buyerEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subTotal, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        SubTotal = subTotal;
        PaymentIntentId = paymentIntentId;
    }

    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ShippingAddress ShippingAddress { get; set; }

    public int? DeliveryMethodId { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
    //public IEnumerable<OrderItem> OrderItems { get; set; }

    public decimal SubTotal { get; set; }
    public decimal geetTotal() => SubTotal + DeliveryMethod.Cost;
    public string PaymentIntentId { get; set; }
}