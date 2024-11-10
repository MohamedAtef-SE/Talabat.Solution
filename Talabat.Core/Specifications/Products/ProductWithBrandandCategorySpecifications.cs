using Talabat.Core.Entities.Products;

namespace Talabat.Core.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(
                  P => (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value)
                       &&
                       (!specParams.CategoryId.HasValue || P.ProductCategoryId == specParams.CategoryId.Value)
                       &&
                       (string.IsNullOrEmpty(specParams.Search) || P.Name.Contains(specParams.Search))

                        , specParams.Sort)

        {
            // AddIncludes() already invoked through base()
            // SortedBy(sort) already invoked through base()

            ApplyPagination(specParams.PageIndex, specParams.PageSize);
        }


        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
        {
            // AddIncludes() already invoked through base()
        }

        protected override void AddIncludes()
        {

            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }


        protected override void SortedBy(string? sort)
        {
            switch (sort)
            {
                case "priceAsc":
                    OrderBy = P => P.Price;
                    break;
                case "priceDesc":
                    OrderByDesc = P => P.Price;
                    break;
                case "nameAsc":
                    OrderBy = P => P.Name;
                    break;
                case "nameDesc":
                    OrderByDesc = P => P.Name;
                    break;
                default:
                    OrderBy = P => P.Id;
                    break;
            }
        }

        private void ApplyPagination(int pageIndex, int pageSize)
        {
            PaginationApplied = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }


    }
}
