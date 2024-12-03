namespace Talabat.Shared.DTOModels.Products
{
    public class ProductDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal? Price { get; set; }
        public string? BrandId { get; set; }
        public string? CategoryId { get; set; }
        public string? Brand { get; set; } = null!;
        public string? Category { get; set; } = null!;

    }
}
