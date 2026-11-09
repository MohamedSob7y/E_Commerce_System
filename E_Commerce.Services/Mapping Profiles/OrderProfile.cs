using AutoMapper;
using E_Commerce.Domain.Entityes.OrderModule;
using E_Commerce.Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Mapping_Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(
                Dest => Dest.DeliveryMethod
                , options => options.MapFrom(src => src.DeliveryMethod.ShortName));//Items مش نفس النوع فى الاتنين + address مش نفس النوع فى الاتنين برضو 
            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(dest => dest.ProductName, op => op.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());//this pictureUrl is needed For Value Resolver=> return nameof Image+Path For User
            CreateMap<DeliveryMethod, DeliveryMethodDto>();

        }
    }
}
