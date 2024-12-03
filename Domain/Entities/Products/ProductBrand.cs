using Talabat.Core.Domain.Entities._Common;

namespace Talabat.Core.Domain.Entities.Products
{
    public class ProductBrand : BaseAuditableEntity<string>
    {
        public required string Name { get; set; }
        // No access Need
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
