using Core;
using Core.Dtos.Carts;
using Core.Entities.Order;
using Core.Services.Contracts;
using Core.Services.Contracts.Carts;
using Core.Specifications.Orders;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Products.Product;

namespace Service;

public class PaymentService : IPaymentService
{
    private readonly ICartService _cartService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentService(ICartService cartService, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _cartService = cartService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    public async Task<CartDto> CreateOrUpdatePaymentIntentIdAsync(string cartId)
    {
        StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

        var cartDto = await _cartService.GetCartAsync(cartId);
        if (cartDto is null) return null;


        var shippingPrice = 0m;

        if (cartDto.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(cartDto.DeliveryMethodId.Value);
            shippingPrice = deliveryMethod.Cost;
        }

        if (cartDto.Items.Count() > 0)
        {
            foreach (var item in cartDto.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
            }
        }

        var subTotal = cartDto.Items.Sum(x => x.Price * x.Quantity);

        var paymentIntentService = new PaymentIntentService();

        PaymentIntent paymentIntent;

        if (string.IsNullOrEmpty(cartDto.PaymentIntentId))
        {
            // Create

            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)(subTotal * 100 + shippingPrice * 100),
                PaymentMethodTypes = new List<string>() { "card" },
                Currency = "usd"
            };

            paymentIntent = await paymentIntentService.CreateAsync(options);
            cartDto.PaymentIntentId = paymentIntent.Id;
            cartDto.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            // Update
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)(subTotal * 100 + shippingPrice * 100),
                //PaymentMethodTypes = new List<string>() { "card" },
                //Currency = "usd"
            };

            paymentIntent = await paymentIntentService.UpdateAsync(cartDto.PaymentIntentId, options);
            cartDto.PaymentIntentId = paymentIntent.Id;
            cartDto.ClientSecret = paymentIntent.ClientSecret;

        }

        cartDto = await _cartService.UpdateCartAsync(cartDto);
        if (cartDto is null)
        {
            return null;
        }
        return cartDto;


    }

    public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
    {
        var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
        var order = await _unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(spec);

        if (flag)
        {
            order.Status = OrderStatus.PaymentReceived;
        }
        else
        {
            order.Status = OrderStatus.PaymentFailed;
        }
        _unitOfWork.GetRepository<Order, int>().Update(order);
        await _unitOfWork.CompleteAsync();
        return order;

    }
}
