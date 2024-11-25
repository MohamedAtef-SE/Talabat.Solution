namespace Talabat.Core.Domain.Entities.Basket
{
    public class BasketItem
    {
        public string Id { get; set; } = null!;
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ProductBrandId { get; set; }
        public string? ProductCategoryId { get; set; }
    }
}