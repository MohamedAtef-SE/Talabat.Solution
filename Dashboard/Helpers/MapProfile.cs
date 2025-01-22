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

        }
    }
}
