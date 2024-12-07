using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.DTOModels.Basket;
using Talabat.Shared.DTOModels.Orders;
using Talabat.Shared.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class PaymentsController(IServiceManager _serviceMaanger) : ControllerBase
    {
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntentEndPoint(string basketId)
        {
            var result = await _serviceMaanger.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            
            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while creating your basket..."));
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_58abc1f44bfe37f5c9e40db89cc7708398e3961ee7c2f062036ac04bac6c1067";

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                var orderDTO = new OrderDTO();
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
              
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                   orderDTO = await _serviceMaanger.PaymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent!.Id, true);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    // Then define and call a method to handle the failed payment intent.
                    // handlePaymentIntentFailed(paymentIntent);
                    orderDTO = await _serviceMaanger.PaymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent!.Id, false); 
                }
                
                return Ok(orderDTO);
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

    }
}
