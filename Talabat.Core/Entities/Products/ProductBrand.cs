namespace Talabat.Core.Application.Entities.Products
{
    public class ProductBrand : BaseEntity
    {

        // No access Need
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
