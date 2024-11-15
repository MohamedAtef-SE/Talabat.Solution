using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Application.Entities.Products;

namespace Talabat.Repository.Data.Configs.Products
{
    internal class ProductCategoryConfigurations : BaseConfigurations<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.HasMany(C => C.Products)
                   .WithOne(P => P.Category)
                   .HasForeignKey(P => P.ProductCategoryId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
