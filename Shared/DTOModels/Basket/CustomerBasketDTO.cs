using System.ComponentModel.DataAnnotations;

namespace Talabat.Shared.DTOModels.Basket
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; } = null!;
        public IEnumerable<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();
        public string? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
