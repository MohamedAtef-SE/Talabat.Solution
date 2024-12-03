using System.Text.Json.Serialization;

namespace Talabat.Core.Domain.Entities.Basket
{
    public class BasketItem
    {

        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("pictureUrl")]
        public string? PictureUrl { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("productBrandId")]
        public string? ProductBrandId { get; set; }

        [JsonPropertyName("productCategoryId")]
        public string? ProductCategoryId { get; set; }
    }
}