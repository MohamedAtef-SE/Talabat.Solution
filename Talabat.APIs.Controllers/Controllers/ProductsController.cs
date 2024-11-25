using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.Errors;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Core.Specifications.Products;

namespace Talabat.APIs.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductCategory> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> brandRepository, IGenericRepository<ProductCategory> categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var productsCountSpec = new ProductsCountForCriteriaSpecification(specParams.BrandId, specParams.CategoryId,specParams.Search);
            var result = await _productRepository.GetAllWithSpecAsync(productsCountSpec);
            var productsCount = result.Count();

            if (productsCount > specParams.PageSize)
            {
                if (specParams.PageIndex > (productsCount / specParams.PageSize))
                {
                    if (productsCount % specParams.PageSize > 0)
                        specParams.PageIndex = (productsCount / specParams.PageSize) + 1;

                    else
                        specParams.PageIndex = productsCount / specParams.PageSize;
                }
                else
                    specParams.PageIndex = specParams.PageIndex;
            }
            else
                specParams.PageIndex = 1;


            var specs = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _productRepository.GetAllWithSpecAsync(specs);
            var productsDTO = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);


            var pagination = new Pagination<ProductDTO>()
            {
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize,
                Count = productsCount,
                Data = productsDTO,
            };

            return Ok(pagination);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTO>> GetProduct(string id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _productRepository.GetWithSpecAsync(specs);

            if (product is not { }) return NotFound(new APIErrorResponse(404));

            var productDTO = _mapper.Map<Product, ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTO>>> GetBrands()
        {
            var brands = await _brandRepository.GetAllAsync();
            var mappedBrands = _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<BrandDTO>>(brands);

            return Ok(mappedBrands);
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var mappedCategories = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<CategoryDTO>>(categories);

            return Ok(mappedCategories);
        }
    }
}
