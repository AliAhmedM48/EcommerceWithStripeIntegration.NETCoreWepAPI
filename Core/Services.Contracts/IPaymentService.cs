using Core.Dtos.Carts;
using Core.Entities.Order;

namespace Core.Services.Contracts;

public interface IPaymentService
{
    Task<CartDto> CreateOrUpdatePaymentIntentIdAsync(string cartId);
    Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);
}
