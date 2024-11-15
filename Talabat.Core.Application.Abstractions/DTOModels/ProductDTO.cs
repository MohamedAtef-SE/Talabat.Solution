namespace Talabat.Core.Application.Abstractions.DTOModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal? Price { get; set; }
        public int? BrandId { get; set; } 
        public int? CategoryId { get; set; }
        public string? Brand { get; set; } = null!; 
        public string? Category { get; set; } = null!; 

    }
}
