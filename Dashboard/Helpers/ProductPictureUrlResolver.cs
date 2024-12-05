using AutoMapper;
using Dashboard.ViewModels;
using Talabat.Core.Domain.Entities.Products;

namespace Route.Talabat.Dashboard.Helpers
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductViewModel<string>, string?>
    {
        public string? Resolve(Product source, ProductViewModel<string> destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["URLs:baseURL"]}/{source.PictureUrl}";
            return string.Empty;
        }
    }
}
