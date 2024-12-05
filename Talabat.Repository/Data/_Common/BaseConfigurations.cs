using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities._Common;
using Talabat.Infrastructure.Persistence._Common;

namespace Talabat.Infrastructure.Persistence.Data._Common
{
    [DBContextType(typeof(StoreContext))]
    internal class BaseConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(E => E.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(E => E.CreatedBy)
                   .IsRequired();

            builder.Property(E => E.CreatedOn)
                   .IsRequired();

            builder.Property(E => E.lastModifedBy)
                   .IsRequired();

            builder.Property(E => E.lastModifedOn)
                   .IsRequired();

        }
    }
}
