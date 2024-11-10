using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Controllers.DTOModels
{
    public class BrandDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> Products { get; set; } = new List<string>();
    }
}
