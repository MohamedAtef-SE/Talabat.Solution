namespace Talabat.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public IEnumerable<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
