using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Products;

namespace Talabat.Repository.Data.Configs.Products
{
    internal class ProductBrandConfigurations : BaseConfigurations<ProductBrand>
    {
        public override void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            base.Configure(builder);

                   
        }
    }
}
