using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Orders
{
    internal class OrderItemConfigurations : BaseConfigurations<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);

            builder.Property(orderItem=> orderItem.Price)
                   .HasColumnType("decimal(18,2)");

            builder.OwnsOne(orderItem => orderItem.Product,product => product.WithOwner());
        }
    }
}