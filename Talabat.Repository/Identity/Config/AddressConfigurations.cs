using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Infrastructure.Persistence._Common;

namespace Talabat.Infrastructure.Persistence.Identity.Config
{
    [DBContextType(typeof(AppIdentityDbContext))]
    internal class AddressConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(address => address.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

        }
    }
}
