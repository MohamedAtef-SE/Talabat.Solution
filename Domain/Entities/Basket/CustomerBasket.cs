using System.Text.Json.Serialization;

namespace Talabat.Core.Domain.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket() { }
        public CustomerBasket(string id)
        {
            Id = id;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("items")]
        public IEnumerable<BasketItem> Items { get; set; } = new List<BasketItem>();

        [JsonPropertyName("deliveryMethodId")]
        public string? DeliveryMethodId { get; set; }

        [JsonPropertyName("paymentIntentId")]
        public string? PaymentIntentId { get; set; }

        [JsonPropertyName("clientSecret")]
        public string? ClientSecret { get; set; }
    }
}
