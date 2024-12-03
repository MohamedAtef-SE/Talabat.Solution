using Talabat.Core.Application.Abstractions._Common;
using Talabat.Shared.DTOModels.Products;


namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<Pagination<ProductDTO>> GetProductsAsync(ProductSpecParams specParams);
        Task<ProductDTO> GetProductAsync(string productId);
        Task<IReadOnlyList<BrandDTO>> GetBrandsAsync();
        Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync();
    }
}