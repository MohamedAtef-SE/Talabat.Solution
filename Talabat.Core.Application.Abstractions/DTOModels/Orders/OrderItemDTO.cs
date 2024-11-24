namespace Talabat.Core.Application.Abstractions.DTOModels.Orders
{
    public class OrderItemDTO
    {
        public string Id { get; set; } = null!;
        public OrderedProductItemDTO Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
