namespace Talabat.Core.Domain.Entities.Products
{
    public  class Product : BaseAuditableEntity<string>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string? ProductBrandId { get; set; } // FK
        public string? ProductCategoryId { get; set; } // FK
        public virtual ProductBrand? Brand { get; set; } = null!; // Navigational Property
        public virtual ProductCategory? Category { get; set; } = null!; // Navigational Property
    }
}
