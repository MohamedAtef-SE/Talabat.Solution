using Microsoft.EntityFrameworkCore;
using Talabat.Core.Application.Entities.Products;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformation).Assembly);
        }

        public DbSet<Product> Product {  get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
    }
}
