using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence._Common;
using Talabat.Repository.Data;

namespace Talabat.Infrastructure.Persistence.Data.Configs
{
    [DBContextType(typeof(StoreContext))]
    internal class BaseConfigurations<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(E => E.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

        }
    }
}
