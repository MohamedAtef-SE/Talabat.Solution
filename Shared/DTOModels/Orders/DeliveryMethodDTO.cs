namespace Talabat.Shared.DTOModels.Orders
{
    public class DeliveryMethodDTO
    {
        public string Id { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
