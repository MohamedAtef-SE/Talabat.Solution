using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Abstractions.Errors;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Basket;


namespace Talabat.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket([FromQuery]string? id)
        {
            if (id is null) return BadRequest(new APIErrorResponse(400));

            var basket = await _basketRepository.GetBasketAsync(id);

            return basket is null ? new CustomerBasket(id) : basket;

        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO customerBasket)
        {
            var basket = await _basketRepository.UpdateBasketAsync(customerBasket);

            if(basket is null) return BadRequest(new APIErrorResponse(400));

            return Ok(basket);
        }


        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string? basketId)
        {

            if (basketId is null) return BadRequest(new APIErrorResponse(400));
           var isDeleted = await _basketRepository.DeleteBasketAsync(basketId);
            return isDeleted ? Ok(new { message = "Deleted Successfully", StatusCode = 200 }) : NotFound(new APIErrorResponse(404,"Something went wrong 😥")); 

        }
    }
}
