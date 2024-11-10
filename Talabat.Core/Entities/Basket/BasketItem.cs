namespace Talabat.Core.Entities.Basket
{
    public class BasketItem
    {
        int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? ProductBrandId { get; set; }
        public int? ProductCategoryId { get; set; }
    }
}