using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.DTOModels.Orders;
using Talabat.Shared.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(OrderDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDTO?>> CreateOrder(CreateOrderDTO createOrder)
        {
            createOrder.BuyerEmail =  User.FindFirstValue(ClaimTypes.Email)!;

            var result = await _serviceManager.OrderService.CreateOrderAsync(createOrder);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while creating your order..."));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersForUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return BadRequest(new APIErrorResponse(400, "Something wrong!! email not exist"));

            var result = await _serviceManager.OrderService.GetOrdersForSpecificUserAsync(userEmail!);

            return result is not null ? Ok(result) 
                                        :
                                       BadRequest(new APIErrorResponse(400, $"Issues found while getting {userEmail!.Split('@')[0]} orders."));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> getOrderDetailed(string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return BadRequest(new APIErrorResponse(400, "Something wrong!! email not exist"));

            var result = await _serviceManager.OrderService.GetOrderByIdAsync(userEmail,id);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, $"Issues found while getting {userEmail.Split('@')[0]} order details."));
        }

        [HttpGet("deliveryMethods")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while getting delivery methods..."));
        }
    }
}
