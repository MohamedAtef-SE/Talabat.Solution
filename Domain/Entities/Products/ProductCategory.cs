namespace Talabat.Core.Domain.Entities.Products
{
    public class ProductCategory : BaseEntity
    {
        // No access need
        public required string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
