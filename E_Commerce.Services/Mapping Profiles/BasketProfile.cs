using AutoMapper;
using E_Commerce.Domain.Entityes.Basket_Module;
using E_Commerce.Shared.DTOS.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Mapping_Profiles
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketDTO,CustomerBasket>().ReverseMap();
            //لازم اتاكد ان اللى Property اللى موجودة فى الDTO هى هى بنفس Case Sensetive اللى موجودة فى الEntity الى موجود فى الDatabase 
            //DTO == Entityin Domain كدة مفيش اى Logic تانى 
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();//دا عشان BasketDTO عندها Property اسمها ICollection<BasketItemDto> + CustomerBasket has Property ICollection<BasketItem>
        }
    }
}
