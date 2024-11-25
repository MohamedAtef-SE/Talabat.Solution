namespace Talabat.Core.Application.Abstractions.DTOModels.Orders
{
    public class OrderedProductItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? PictureUrl { get; set; }
    }
}
