using AutoMapper;
using AutoMapper.Configuration.Annotations;
using AutoMapper.Execution;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Controllers.DTOModels;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Controllers.Mapping
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
