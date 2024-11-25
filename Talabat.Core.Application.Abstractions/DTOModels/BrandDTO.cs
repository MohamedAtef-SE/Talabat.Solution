namespace Talabat.Core.Application.Abstractions.DTOModels
{
    public class BrandDTO
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public List<string> Products { get; set; } = new List<string>();
    }
}
