using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence._Common;

namespace Talabat.Infrastructure.Persistence.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);

           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
                                                        type => type.GetCustomAttribute<DBContextTypeAttribute>()?.DBType == typeof(StoreContext));
        }

        public DbSet<Product> Product {  get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}
