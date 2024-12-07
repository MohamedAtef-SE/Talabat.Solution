using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Application.Specifications.Products
{
    public class ProductsCountForCriteriaSpecification : BaseSpecifications<Product,string>
    {
        public ProductsCountForCriteriaSpecification(string? brandId = null, string? categoryId = null, string? search = null)
            : base(P => (String.IsNullOrEmpty(brandId) || P.ProductBrandId!.Equals(brandId))
                       &&
                      (String.IsNullOrEmpty(categoryId) || P.ProductCategoryId!.Equals(categoryId))
                       &&
                      (string.IsNullOrEmpty(search) || P.Name.Contains(search)))

        {

        }


    }
}
