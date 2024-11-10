using System.Linq.Expressions;
using Talabat.Core.Contracts;
using Talabat.Core.Entities.Products;

namespace Talabat.Core.Specifications.Products
{
    public class ProductsCountForCriteriaSpecification : BaseSpecifications<Product>
    {
        public ProductsCountForCriteriaSpecification(int? brandId = null,int? categoryId = null,string? search = null)
            :base(P => (!brandId.HasValue || P.ProductBrandId == brandId.Value)
                       &&
                      (!categoryId.HasValue || P.ProductCategoryId == categoryId.Value)
                       &&
                      (string.IsNullOrEmpty(search) || P.Name.Contains(search)))
        
        {
        
        }

       
    }
}
