using Talabat.Core.Domain.Entities._Common;

namespace Talabat.Core.Domain.Entities.Products
{
    public class ProductCategory : BaseAuditableEntity<string>
    {
        // No access need
        public required string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
