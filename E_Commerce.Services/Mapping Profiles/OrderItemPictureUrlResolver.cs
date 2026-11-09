using AutoMapper;
using E_Commerce.Domain.Entityes.OrderModule;
using E_Commerce.Shared.DTOS.OrderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Mapping_Profiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItems, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItems source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))//عشان اتاكد ان الصورة موجودة اصلا ولا لاء فى الDb

                return string.Empty;

            //لو الPath بتاع الصورة موجودة على server تانى مش محتاج احط ال BaseUrl لانها جاية بالFullPath بتاعها 
            //انما لو موجودة عندى على الServer in Db محتاج اعملها BaseUrl وارجعها للUser
            if (source.Product.PictureUrl.StartsWith("http") ||
                source.Product.PictureUrl.StartsWith("https"))
            {
                return source.Product.PictureUrl;
            }

            var baseurl = _configuration.GetSection("URLs")["BaseUrl"];
            if (string.IsNullOrEmpty(baseurl))
                return string.Empty;
            return $"{baseurl}{source.Product.PictureUrl}";
        }
    }
}
