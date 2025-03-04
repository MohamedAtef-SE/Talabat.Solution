﻿using Talabat.Core.Domain.Entities.Products;
using Talabat.Shared.DTOModels.Products;

namespace Talabat.Core.Application.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product,string>
    {

        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(
                  P => (String.IsNullOrEmpty(specParams.BrandId) || P.ProductBrandId!.Equals(specParams.BrandId))
                       &&
                       (String.IsNullOrEmpty(specParams.CategoryId) || P.ProductCategoryId!.Equals(specParams.CategoryId))
                       &&
                       (string.IsNullOrEmpty(specParams.Search) || P.Name.Contains(specParams.Search))

                        , specParams.Sort)

        {
            // AddIncludes() already invoked through base()
            // SortedBy(sort) already invoked through base()

            ApplyPagination(specParams.PageIndex, specParams.PageSize);
        }


        public ProductWithBrandAndCategorySpecifications(string id) : base(P => P.Id.Equals(id))
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
