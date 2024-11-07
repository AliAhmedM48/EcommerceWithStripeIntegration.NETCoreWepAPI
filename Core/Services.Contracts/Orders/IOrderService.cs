using Core.Entities.Order;

namespace Core.Services.Contracts.Orders;
public interface IOrderService
{
    Task<Order> CreateOrderAsync(string buyerEmail, string cartId, int deliveryMethodId, ShippingAddress shippingAddress);
    Task<IEnumerable<Order>?> GetAllOrdersForSpecigicUserAsync(string buyerEmail);
    Task<Order?> GetOneOrderByIdForSpecigicUserAsync(string buyerEmail, int orderId);
}

