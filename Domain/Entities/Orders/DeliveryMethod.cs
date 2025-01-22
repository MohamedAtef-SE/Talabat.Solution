namespace Talabat.Core.Domain.Entities.Orders
{
    public class DeliveryMethod : BaseAuditableEntity<string>
    {
        public required string ShortName { get; set; }
        public string Description { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
