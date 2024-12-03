namespace Talabat.Core.Domain.Entities.Orders
{
    public class OrderItem : BaseAuditableEntity<string>
    {
        // Parameterless Constructor For EF Core
        public OrderItem() { }

        public OrderItem(OrderedProductItem product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public OrderedProductItem Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // No need to access
        public string OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
    }
}
