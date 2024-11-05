using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configs
{
    internal class BaseConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(E => E.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(B => B.Name)
                   .IsRequired();
                   
        }
    }
}
