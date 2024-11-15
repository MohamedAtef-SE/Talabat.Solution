using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Application.Entities.Products;

namespace Talabat.Repository.Data.Configs.Products
{
    internal class ProductBrandConfigurations : BaseConfigurations<ProductBrand>
    {
        public override void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            base.Configure(builder);

            builder.HasMany(B => B.Products)
                .WithOne(P => P.Brand)
                .HasForeignKey(P => P.ProductBrandId)
                .OnDelete(DeleteBehavior.SetNull);       
        }
    }
}
