using Core.Entities.Order;

namespace Core.Specifications.Orders;

public class OrderSpecificationWithPaymentIntentId : BaseSpecifications<Order, int>
{
    public OrderSpecificationWithPaymentIntentId(string paymentIntentId) : base(x => x.PaymentIntentId == paymentIntentId)
    {
        Includes.Add(x => x.DeliveryMethod);
        Includes.Add(x => x.OrderItems);
    }
}
