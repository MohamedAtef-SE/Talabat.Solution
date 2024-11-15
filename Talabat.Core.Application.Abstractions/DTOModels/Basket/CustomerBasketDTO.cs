using System.ComponentModel.DataAnnotations;

namespace Talabat.Core.Application.Abstractions.DTOModels.Basket
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }
        public IEnumerable<BasketItemDTO> Items { get; set; } = new List<BasketItemDTO>();
    }
}
