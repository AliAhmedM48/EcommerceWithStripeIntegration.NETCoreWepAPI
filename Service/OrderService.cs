using Core;
using Core.Entities.Order;
using Core.Entities.Products;
using Core.Services.Contracts;
using Core.Services.Contracts.Carts;
using Core.Services.Contracts.Orders;
using Core.Specifications.Orders;

namespace Service;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;

    public OrderService(IUnitOfWork unitOfWork, ICartService cartService, IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _paymentService = paymentService;
    }
    public async Task<Order> CreateOrderAsync(string buyerEmail, string cartId, int deliveryMethodId, ShippingAddress shippingAddress)
    {
        var cartDto = await _cartService.GetCartAsync(cartId);
        //if (cartDto == null) throw new InvalidOperationException("no products in the cart!");
        if (cartDto == null) return null;

        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

        var orderItems = new List<OrderItem>();

        foreach (var cartItemDto in cartDto.Items)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(cartItemDto.Id);
            var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
            var orderItem = new OrderItem(productItemOrder, product.Price, cartItemDto.Quantity);
            orderItems.Add(orderItem);
        }
        var subTotal = orderItems.Aggregate(0m, (total, item) => total + (item.Price * item.Quantity));
        //var subTotal = orderItems.Sum(i => i.Price * i.Quantity);


        if (!string.IsNullOrEmpty(cartDto.PaymentIntentId))
        {
            var spec = new OrderSpecificationWithPaymentIntentId(cartDto.PaymentIntentId);
            var ExOrder = await _unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(spec);
            if (ExOrder is not null)
            {

                _unitOfWork.GetRepository<Order, int>().Delete(ExOrder);
            }
        }

        cartDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(cartId);

        var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, cartDto.PaymentIntentId);

        await _unitOfWork.GetRepository<Order, int>().AddAsync(order);
        var result = await _unitOfWork.CompleteAsync();
        if (result <= 0) return null;
        return order;
    }

    public async Task<Order?> GetOneOrderByIdForSpecigicUserAsync(string buyerEmail, int orderId)
    {
        var spec = new OrderSpecifications(buyerEmail, orderId);
        var order = await _unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(spec);
        if (order is null) return null;
        return order;
    }

    public async Task<IEnumerable<Order>?> GetAllOrdersForSpecigicUserAsync(string buyerEmail)
    {
        var spec = new OrderSpecifications(buyerEmail);
        var orders = await _unitOfWork.GetRepository<Order, int>().GetAllWithSpecAsync(spec);
        if (orders is null) return null;
        return orders;
    }
}
