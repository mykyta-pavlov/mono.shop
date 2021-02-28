using System.Collections.Generic;
using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(IConfiguration config)
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ThumbnailUrl, o => o.MapFrom(s => config["ApiUrl"] + s.ThumbnailUrl))
                .ForMember(d => d.ImageUrls,
                    o => o.MapFrom(s => s.ImageUrls.Select(x => config["ApiUrl"] + x.ImageUrl)))
                .ForMember(d => d.SizesAvailable,
                    o => o.MapFrom(s =>
                        s.ProductSizeQuantity.Select(x => new KeyValuePair<string, bool>(x.SizeName, x.Quantity != 0))));

            CreateMap<ProductType, TypesDto>()
                .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.ProductCategory.Name));
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.ThumbnailUrl, o => o.MapFrom(s => config["ApiUrl"] + s.ItemOrdered.ThumbnailUrl));
            //.ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}