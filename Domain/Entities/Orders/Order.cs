using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        // Parameterless Constructor For EF Core
        public Order() { }

        public Order(string buyerEmail, Address shippingAddress, string deliveryMethodId, ICollection<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethodId = deliveryMethodId;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } = null!;
        public string DeliveryMethodId { get; set; } // FK
        public virtual DeliveryMethod DeliveryMethod { get; set; } = null!; // Navigational Property [1-1]
        public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property [1-M]
        public decimal Subtotal { get; set; }

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost;
        // OR use Getter Method
        public decimal GetTotal() => DeliveryMethod.Cost + Subtotal;
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}