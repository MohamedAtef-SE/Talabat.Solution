using AutoMapper;
using Talabat.APIs.Controllers.DTOModels;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Controllers.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(P => P.Brand, O => O.MapFrom(P => P.Brand!.Name))
                .ForMember(P => P.Category, O => O.MapFrom(P => P.Category!.Name))
                .ForMember(P => P.PictureUrl,O=>O.MapFrom<ImageUrlResolver>());
        }
    }
}
