using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Orders
{
    internal class OrderConfigurations : BaseConfigurations<Order,string>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(order => order.Status)
                   .HasConversion(
                orderStatus => orderStatus.ToString(),
                orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
                );

            builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.HasOne(order => order.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(order=> order.Items)
                   .WithOne(OI => OI.Order)
                   .HasForeignKey(OI => OI.OrderId);

            builder.Property(order => order.Subtotal)
                    .HasColumnType("decimal(18,2)");

        }
    }
}
