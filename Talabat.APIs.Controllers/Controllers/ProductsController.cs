using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Contracts;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _genericRepository;

        public ProductsController(IGenericRepository<Product> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _genericRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _genericRepository.GetAsync(id);

            return Ok(product);
        }
    }
}
