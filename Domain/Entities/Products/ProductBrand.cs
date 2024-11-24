namespace Talabat.Core.Domain.Entities.Products
{
    public class ProductBrand : BaseEntity
    {
        public required string Name { get; set; }
        // No access Need
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
