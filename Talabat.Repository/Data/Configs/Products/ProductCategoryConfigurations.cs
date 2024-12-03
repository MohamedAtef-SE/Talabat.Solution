using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Products
{
    internal class ProductCategoryConfigurations : BaseConfigurations<ProductCategory,string>
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
