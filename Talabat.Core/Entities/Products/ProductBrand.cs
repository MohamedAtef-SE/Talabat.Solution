namespace Talabat.Core.Entities.Products
{
    public class ProductBrand : BaseEntity
    {

        // No access Need
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
