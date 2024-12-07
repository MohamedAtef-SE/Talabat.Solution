using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Controllers.Attributes;
using Talabat.Core.Application.Abstractions._Common;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Shared.DTOModels.Products;
using Talabat.Shared.Errors;

namespace Talabat.APIs.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IServiceManager _serviceManager, IMapper _mapper) : ControllerBase
    {
        

        [Cached(2)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var result = await _serviceManager.ProductService.GetProductsAsync(specParams);

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400,"Fetching products failed 😔."));
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(string id)
        {
           var result = await _serviceManager.ProductService.GetProductAsync(id);

            return result is not null ? Ok(result) : NotFound(new APIErrorResponse(404,$"No Product with this Id: {id} found..."));
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetBrands()
        {
           var result = await _serviceManager.ProductService.GetBrandsAsync();

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Fetching brands failed 😔."));
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDTO>>> GetCategories()
        {
            var result = await _serviceManager.ProductService.GetCategoriesAsync();

            return result is not null ? Ok(result) : BadRequest(new APIErrorResponse(400, "Fetching categories failed 😔."));
        }
    }
}
