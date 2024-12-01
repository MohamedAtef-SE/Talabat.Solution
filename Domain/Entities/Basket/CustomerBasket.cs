namespace Talabat.Core.Domain.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public IEnumerable<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
