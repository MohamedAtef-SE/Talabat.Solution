using AutoMapper;
using Dashboard.ViewModels;
using Talabat.Core.Domain.Entities.Products;

namespace Route.Talabat.Dashboard.Helpers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<ProductViewModel<string>, Product>().ReverseMap()
            .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            //CreateMap<Product, ProductViewModel<string>>()
            //   .ForMember(d => d.ProductBrand, O => O.MapFrom(src => src.Brand!.Name))
            //   .ForMember(d => d.ProductCategory, O => O.MapFrom(src => src.Category!.Name))
            //   .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>()).ReverseMap();


        }
    }
}
