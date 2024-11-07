using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Controllers.DTOModels;
using Talabat.APIs.Controllers.Errors;
using Talabat.Core.Contracts;
using Talabat.Core.Entities.Products;
using Talabat.Core.Specifications.Products;

namespace Talabat.APIs.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var specs = new ProductWithBrandAndCategorySpecifications();
            var products = await _genericRepository.GetAllWithSpecAsync(specs);

            var productsDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            
            var product = await _genericRepository.GetWithSpecAsync(specs);
            
            if (product is not { }) return NotFound(new APIErrorResponse(404));

            var productDTO = _mapper.Map<Product, ProductDTO>(product);

            return Ok(productDTO);
        }
    }
}
