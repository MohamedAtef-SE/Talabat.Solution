using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Products;

namespace Talabat.Repository.Data.Configs.Products
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
                   .WithMany()
                   .HasForeignKey(P => P.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(P => P.Brand)
                   .WithMany()
                   .HasForeignKey(P => P.BrandId)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
