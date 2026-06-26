using AutoMapper;
using E_Commerce.Domain.Entityes;
using E_Commerce.Shared.DTOS;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace E_Commerce.Services.Mapping_Profiles
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;
        //Ask Clr To inject Object From any class implement interface IConfiguration => فلازم اعمله داخل الMain
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            //Check Source return Picture Url اصلا قبل اعمل اى حاجة 
            if(string.IsNullOrEmpty(source.PictureURL)) return string.Empty;
             //Check انها مش جاية بالFullPath عشان ممكن تكون جايالى بالFullPath فعشان كدة محتاج اتااكد الاول عشان لو مش بالFullPath اعملها الFullPath كامل
             //فى حالة ان الصور موجودة على server تانى يبقى كدة انا خزنت الURl كامل فى Db وبترجعلى بالFullPath بتاعها فمش محتاج اعمل حاجة اروح عاملها upload on server وخلاص مش محتاج اى ValueResolver 
             //انما لو الصور كانت معموليها seeding يبقى اخزن الUrl بتاعها لان الصور دى محتاج اعملها Upload on Server باالتلى محتاج ترجعلى الfullPath وانا هنا مش هترجعلى الfullPath عشان كدة اكونه انا 
             if(source.PictureURL.StartsWith("http")||source.PictureURL.StartsWith("https")) return source.PictureURL;//this FullPath مش محتاج اعمل حاجة كدة انما لو مش بترجع بنفس الحاجة دى محتاج اكون انا الPath 
             //var Pictureurl=$"{"https://localhost:7226"}/{source.PictureURL}";//so Full Path اللى هيرجعلى بعد كدة هيبقى عامل كدة https://localhost:7226/images/Products/nameofImage 
            //  بس هنا فى مشكلة => Url متغير برضو وكدة انا معملتشى اى حاجة لانه متغير من enviornment للتانيه عشان كدة احطه فى فايل //Appsetting وانادى عليه هنا 
            //Solve this Problem انى انادى عليه بقا => 
            //عايز اوصل للBaseUrl اللى موجود فى الAppsetting
            var baseurl=_configuration.GetSection("URLs")["BaseUrl"];
            var Pictureurl=$"{baseurl}{source.PictureURL}";
            return Pictureurl;
        }
    }
}
