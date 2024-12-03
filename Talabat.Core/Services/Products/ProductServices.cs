using AutoMapper;
using Microsoft.Extensions.Logging;
using Talabat.Core.Application.Abstractions._Common;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Specifications.Products;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Shared.DTOModels.Products;
using Talabat.Shared.Exceptions;

namespace Talabat.Core.Application.Services.Products
{
    internal class ProductServices(IUnitOfWork _unitOfWork,ILogger<ProductServices> _logger,IMapper _mapper) : IProductService
    {
        public async Task<Pagination<ProductDTO>> GetProductsAsync(ProductSpecParams specParams)
        {
            try
            {
                var productsCountSpec = new ProductsCountForCriteriaSpecification(specParams.BrandId, specParams.CategoryId, specParams.Search);
                var productsAppliedTheCriteria = await _unitOfWork.GetRepository<Product,string>().GetAllWithSpecAsync(productsCountSpec);
                var productsCount = productsAppliedTheCriteria.Count();

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
                var products = await _unitOfWork.GetRepository<Product,string>().GetAllWithSpecAsync(specs);
                var productsDTO = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);


                var pagination = new Pagination<ProductDTO>()
                {
                    PageIndex = specParams.PageIndex,
                    PageSize = specParams.PageSize,
                    Count = productsCount,
                    Data = productsDTO,
                };

                return pagination;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);

                throw new BadRequestException(ex.Message);
            }
            
        }
        public async Task<ProductDTO> GetProductAsync(string id)
        { 
            var specs = new ProductWithBrandAndCategorySpecifications(id);

            try
            {
                var product = await _unitOfWork.GetRepository<Product,string>().GetWithSpecAsync(specs);

                if (product is not { }) throw new Exception($"No Product Found with this id: {id}");

                var productDTO = _mapper.Map<Product, ProductDTO>(product);

                return productDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<IReadOnlyList<BrandDTO>> GetBrandsAsync()
        {
            try
            {
                var brands = await _unitOfWork.GetRepository<ProductBrand,string>().GetAllAsync();
                var mappedBrands = _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<BrandDTO>>(brands);

                return mappedBrands;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.GetRepository<ProductCategory,string>().GetAllAsync();
                var mappedCategories = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<CategoryDTO>>(categories);

                return mappedCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
