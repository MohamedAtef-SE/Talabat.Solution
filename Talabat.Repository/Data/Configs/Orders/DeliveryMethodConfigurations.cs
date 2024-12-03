using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Orders;

namespace Talabat.Infrastructure.Persistence.Data.Configs.Orders
{
    internal class DeliveryMethodConfigurations : BaseConfigurations<DeliveryMethod,string>
    {
        public override void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            base.Configure(builder);
            builder.Property(D => D.Cost)
                   .HasColumnType("decimal(9,2)");
        }
    }
}
