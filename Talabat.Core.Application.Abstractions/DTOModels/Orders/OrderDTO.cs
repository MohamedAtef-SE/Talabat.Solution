namespace Talabat.Core.Application.Abstractions.DTOModels.Orders
{
    public class OrderDTO
    {
        public string Id { get; set; } = null!;
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset Date { get; set; } 
        public string Status { get; set; } = null!;
        public AddressDTO ShippingAddress { get; set; } = null!;
        //public string DeliveryMethodId { get; set; } = null!; // FK
        public DeliveryMethodDTO DeliveryMethod { get; set; } = null!; 
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>(); 
        public decimal Subtotal { get; set; }
        public decimal GetTotal() => DeliveryMethod.Cost + Subtotal;
        public string PaymentIntentId { get; set; } = string.Empty; 

    }
}