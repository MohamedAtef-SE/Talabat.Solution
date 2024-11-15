using AutoMapper;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Entities.Products;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(P => P.Brand, O => O.MapFrom(P => P.Brand!.Name))
                .ForMember(P => P.Category, O => O.MapFrom(P => P.Category!.Name))
                .ForMember(P => P.PictureUrl,O=>O.MapFrom<ImageUrlResolver>());


            CreateMap<ProductBrand, BrandDTO>()
                .ForMember(des => des.Products, O => O.MapFrom((src, des) =>
                {
                    List<string> nameOfProducts = new List<string>();

                    foreach(var product in src.Products)
                        nameOfProducts.Add(product.Name);
                    return nameOfProducts;
                }));

            CreateMap<ProductCategory, CategoryDTO>()
                .ForMember(des => des.Products, O => O.MapFrom((src, des) =>
                {
                    List<string> nameOfProducts = new List<string>();

                    foreach (var product in src.Products)
                        nameOfProducts.Add(product.Name);
                    return nameOfProducts;
                }));

            CreateMap<Address, AddressDTO>().ReverseMap();

            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();
        }
    }
}
