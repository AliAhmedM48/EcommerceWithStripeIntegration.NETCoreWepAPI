using Core.Services.Contracts;
using demo.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace demo.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{cartId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent(string cartId)
        {
            if (string.IsNullOrEmpty(cartId)) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var cartDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(cartId);
            if (cartDto == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(cartDto);
        }

        const string endpointSecret = "whsec_15d6f944860f02b2946ea443fe4ea7f6b85fab913f15aa7a3b1abb22528ca61c";

        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, flag: true);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodFailed(paymentMethod);
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, flag: false);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
