using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Products
{
    internal class ProductConfigurations : BaseConfigurations<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(P => P.Description)
                   .IsRequired();
                   

            builder.Property(P => P.PictureUrl)
                   .IsRequired();

            builder.Property(P => P.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.Category)
                   .WithMany(C => C.Products)
                   .HasForeignKey(P => P.ProductCategoryId)
                   .OnDelete(DeleteBehavior.SetNull);
            
            builder.HasOne(P => P.Brand)
                   .WithMany(B => B.Products)
                   .HasForeignKey(P => P.ProductBrandId)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
