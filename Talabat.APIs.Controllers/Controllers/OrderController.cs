using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;
using Talabat.Core.Application.Abstractions.Errors;
using Talabat.Core.Application.Abstractions.Services;

namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(OrderDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDTO?>> CreateOrder(OrderParams orderParams)
        {
            orderParams.BuyerEmail =  User.FindFirstValue(ClaimTypes.Email)!;

            var order = await _orderService.CreateOrderAsync(orderParams);

            if (order is null) return BadRequest(new APIErrorResponse(400,"Can't create this order."));

            return Ok(order);
        }

        [HttpGet("UserOrders")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetUserOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUserAsync(userEmail!);

            if (orders?.Count() > 0)
                return NotFound(new APIErrorResponse(404, "no orders found."));  
            
            return Ok(orders);
        }

        [HttpGet("UserSpecificOrder/{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> GetSpecificUserOrder(string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(userEmail,id);

            return Ok(order);
        }

        [HttpGet("deliveryMethods")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetAllDeliveryMethodsAsync();
            if(deliveryMethods?.Count() == 0) return NotFound(new APIErrorResponse(404,"No delivery methods found."));
            return Ok(deliveryMethods);
        }
    }
}
