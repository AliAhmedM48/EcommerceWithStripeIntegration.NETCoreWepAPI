using Core.Entities.Order;

namespace Core.Specifications.Orders;
public class OrderSpecifications : BaseSpecifications<Order, int>
{
    public OrderSpecifications(string buyerEmail, int orderId)
        : base(o => o.BuyerEmail == buyerEmail && o.Id == orderId)
    {
        Includes.Add(o => o.DeliveryMethod);
        Includes.Add(o => o.OrderItems);
    }

    public OrderSpecifications(string buyerEmail)
       : base(o => o.BuyerEmail == buyerEmail)
    {
        Includes.Add(o => o.DeliveryMethod);
        Includes.Add(o => o.OrderItems);
    }
}
