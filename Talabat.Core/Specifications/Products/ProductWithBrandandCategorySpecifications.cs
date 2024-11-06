using Talabat.Core.Entities.Products;

namespace Talabat.Core.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications() : base(null)
        {

        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
        {

        }

        protected override void AddIncludes()
        {

            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }
    }
}
