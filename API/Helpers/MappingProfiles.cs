using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>())
                .ForMember(d => d.SizeAvailableXxs, o => o.MapFrom(s => s.SizeQuantityXxs != 0))
                .ForMember(d => d.SizeAvailableXs, o => o.MapFrom(s => s.SizeQuantityXs != 0))
                .ForMember(d => d.SizeAvailableS, o => o.MapFrom(s => s.SizeQuantityS != 0))
                .ForMember(d => d.SizeAvailableM, o => o.MapFrom(s => s.SizeQuantityM != 0))
                .ForMember(d => d.SizeAvailableL, o => o.MapFrom(s => s.SizeQuantityL != 0))
                .ForMember(d => d.SizeAvailableXl, o => o.MapFrom(s => s.SizeQuantityXl != 0))
                .ForMember(d => d.SizeAvailableXxl, o => o.MapFrom(s => s.SizeQuantityXxl != 0));

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
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}