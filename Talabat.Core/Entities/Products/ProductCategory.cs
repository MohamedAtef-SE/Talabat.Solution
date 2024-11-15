namespace Talabat.Core.Application.Entities.Products
{
    public class ProductCategory : BaseEntity
    {
        // No access need
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
