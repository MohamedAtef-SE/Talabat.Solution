namespace Talabat.Core.Specifications.Products
{
    public class ProductSpecParams
    {
        private int pageIndex = 1;
        private int pageSize = 10;

        public string? Search {  get; set; }
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int PageIndex { get => pageIndex; set => pageIndex = value; }
        public int PageSize { get => pageSize; set => pageSize = value > 10 ? 10: value; }
    }
}
