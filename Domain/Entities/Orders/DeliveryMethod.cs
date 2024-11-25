using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Domain.Entities.Orders
{
    public class DeliveryMethod : BaseEntity
    {
        public required string ShortName { get; set; }
        public string Description { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
