namespace Talabat.Shared.DTOModels.Orders
{
    public class OrderedProductItemDTO
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? PictureUrl { get; set; }
    }
}
