using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Infrastructure.Persistence._Common;

namespace Talabat.Infrastructure.Persistence.Identity.Config
{
    [DBContextType(typeof(AppIdentityDbContext))]
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(U => U.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(U => U.UserName)
                   .IsRequired();
            builder.Property(U => U.DisplayName)
                   .IsRequired();
            builder.Property(U => U.Email)
                   .HasMaxLength(128)
                   .HasColumnType("nvarchar")
                   .IsRequired();
           
            builder.HasOne(U => U.Address)
                   .WithOne(A => A.User)
                   .HasForeignKey<Address>(A => A.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
