namespace Talabat.Core.Domain.Entities.Products
{
    public  class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ProductBrandId { get; set; } // FK
        public string? ProductCategoryId { get; set; } // FK
        public virtual ProductBrand? Brand { get; set; } = null!; // Navigational Property
        public virtual ProductCategory? Category { get; set; } = null!; // Navigational Property
    }
}
