using AutoMapper;
using E_Commerce.Domain.Entityes;
using E_Commerce.Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Mapping_Profiles
{
    internal class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(Src => Src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(Src => Src.ProductType.Name))////These Not Loaded in Memory لازم اعمل Behavoir عشان اخليها Loaded عندى 
            #region Picture URL
            //.ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => $"{"https://localhost:7226"}/{src.PictureURL}")); 
            //مش هينفع حاجة بتتغير من Enviornment للتانيه تتثبت كدة فى الكود دا غلط 

            //======================================================
            //After implement interface IValueResolver 
            .ForMember(dest=>dest.PictureUrl,opt=>opt.MapFrom<ProductPictureUrlResolver>());//Use MapFrom النسخة الGeneric اللى بتاخد Any objectimplement interface IValueResolver عشان اخد الFullPath او الداتا اللى راجعة منه هو لانه بيكون الFullPath 
            #endregion

            //==================================================================================

        }
    }
}
