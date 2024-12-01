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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDTO?>> CreateOrder(CreateOrderDTO createOrder)
        {
            createOrder.BuyerEmail =  User.FindFirstValue(ClaimTypes.Email)!;

            var order = await _orderService.CreateOrderAsync(createOrder);

            if (order is null) return BadRequest(new APIErrorResponse(400,"Can't create this order."));

            return Ok(order);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersForUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUserAsync(userEmail!);

            if (orders?.Count() == 0)
                return NotFound(new APIErrorResponse(404, "no orders found."));  
            
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> getOrderDetailed(string id)
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
