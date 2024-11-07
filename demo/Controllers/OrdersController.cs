using AutoMapper;
using Core;
using Core.Dtos.Orders;
using Core.Entities.Order;
using Core.Services.Contracts.Orders;
using demo.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace demo.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));

            var ShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddressDto);

            var order = await _orderService.CreateOrderAsync(
                buyerEmail: userEmail,
                cartId: orderDto.CartId,
                deliveryMethodId: orderDto.DeliveryMethodId,
                shippingAddress: ShippingAddress);

            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var mappedOrder = _mapper.Map<OrderReturnedDto>(order);

            return Ok(mappedOrder);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderService.GetAllOrdersForSpecigicUserAsync(userEmail);
            if (orders is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var mappedOrders = _mapper.Map<IEnumerable<OrderReturnedDto>>(orders);
            return Ok(mappedOrders);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser(int orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var order = await _orderService.GetOneOrderByIdForSpecigicUserAsync(userEmail, orderId);
            if (order is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var mappedOrders = _mapper.Map<OrderReturnedDto>(order);
            return Ok(mappedOrders);
        }


        [HttpGet("DeliveryMethods")]
        [Authorize]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (deliveryMethods is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(deliveryMethods);
        }
    }
}