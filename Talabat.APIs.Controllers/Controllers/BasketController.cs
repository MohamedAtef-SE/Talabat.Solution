using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.DTOModels.Basket;
using Talabat.Shared.Errors;


namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IBasketService _basketService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CustomerBasketDTO>> GetCustomerBasket([FromQuery] string id)
        {
            var result = await _basketService.GetCustomerBasketAsync(id);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while getting customer basket..."));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDTO>> UpdateBasket(CustomerBasketDTO customerBasket)
        {
            var result = await _basketService.UpdateBasketAsync(customerBasket);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Issues found while updating customer basket..."));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string basketId)
        {
            var result = await _basketService.DeleteBasketAsync(basketId);

            return result is not false ? Ok(new { message = "Deleted successfully 🤝"}) : BadRequest(new APIErrorResponse(400, "Issues found while deleting customer basket..."));
        }
    }
}
