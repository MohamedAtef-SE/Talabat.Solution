namespace Talabat.Core.Entities.Products
{
    public  class Product : BaseEntity
    {
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal? Price { get; set; }
        public int? BrandId { get; set; } // FK
        public int? CategoryId { get; set; } // FK
        public virtual ProductBrand? Brand { get; set; } = null!; // Navigational Property
        public virtual ProductCategory? Category { get; set; } = null!; // Navigational Property
    }
}
