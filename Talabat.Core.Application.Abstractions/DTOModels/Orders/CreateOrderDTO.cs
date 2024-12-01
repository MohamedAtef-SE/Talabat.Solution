namespace Talabat.Core.Application.Abstractions.DTOModels.Orders
{
    public class CreateOrderDTO
    {
        public string BuyerEmail { get; set; } = null!;
        public string BasketId { get; set; } = null!;
        public string DeliveryMethodId { get; set; } = null!;
        public AddressDTO ShippingAddress { get; set; } = null!;

    }
}