using Talabat.Core.Domain.Entities.Products;

namespace Dashboard.ViewModels
{
    public class ProductViewModel<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public string? CreatedBy { get; set; } = null!;
        public DateTimeOffset CreatedOn { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string? ProductBrandId { get; set; }           // ForeignKey ---> ProductBrands Entity
        public string? ProductCategoryId { get; set; }        // ForeignKey ---> ProductCategory Entity
        public IFormFile? Image { get; set; }
        public ProductBrand? ProductBrand { get; set; }       // Navigational Property [ONE]
        public ProductCategory? ProductCategory { get; set; } // Navigational Property [ONE]
    }
}
