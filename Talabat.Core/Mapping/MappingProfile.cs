using AutoMapper;
using Talabat.Core.Application.Abstractions.DTOModels;
using Talabat.Core.Application.Abstractions.DTOModels.Basket;
using Talabat.Core.Application.Abstractions.DTOModels.Orders;
using Talabat.Core.Domain.Entities.Basket;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;

namespace Talabat.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(P => P.Brand, O => O.MapFrom(P => P.Brand!.Name))
                .ForMember(P => P.Category, O => O.MapFrom(P => P.Category!.Name))
                .ForMember(P => P.PictureUrl, O => O.MapFrom<ImageUrlResolver>());


            CreateMap<ProductBrand, BrandDTO>()
                .ForMember(des => des.Products, O => O.MapFrom((src, des) =>
                {
                    List<string> nameOfProducts = new List<string>();

                    foreach (var product in src.Products)
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

            CreateMap<Domain.Entities.Identity.Address, AddressDTO>().ReverseMap();

            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();

            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
            CreateMap<OrderedProductItemDTO, OrderedProductItem>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>()
                .ForMember(orderDTO => orderDTO.Status, options => options.MapFrom(order => order.Status.ToString()))
                .ReverseMap();


        }
    }
}
