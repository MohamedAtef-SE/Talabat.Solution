namespace Talabat.Core.Domain.Entities.Orders
{
    public class OrderedProductItem
    {
        // Parameterless Constructor For EF Core
        public OrderedProductItem() { }

        public OrderedProductItem(string productId, string productName, string? pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? PictureUrl { get; set; }
    }
}
