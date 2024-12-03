using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Shared.DTOModels.Products;

namespace Talabat.Core.Application.Mapping
{
    internal class ImageUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDTO, string?>
    {
        public string? Resolve(Product source, ProductDTO destination, string? destMember, ResolutionContext context)
        {
            destMember = string.Empty;
            
            if(!String.IsNullOrEmpty(source.PictureUrl))
                destMember = $"{configuration.GetSection("URLs")["ApiBaseUrl"]}/{source.PictureUrl}";

            return destMember;
        }
    }
}
