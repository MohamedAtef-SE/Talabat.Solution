﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence.Data._Common;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Products
{
    internal class ProductBrandConfigurations : BaseConfigurations<ProductBrand,string>
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
