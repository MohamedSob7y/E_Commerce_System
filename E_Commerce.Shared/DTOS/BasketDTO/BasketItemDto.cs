//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace E_Commerce.Shared.DTOS.BasketDTO
//{
//    public record BasketItemDto
//        (int Id,
//        string ProductName,
//        string PictureUrl,
//        [Range(0,double.MaxValue)]
//            decimal Price,
//        [Range(0,100)]
//             int Quantity
//        );
//    //So When Createing Object From This Type الValues اللى جواه هتكون Immutable يعنى مش هعرف اعدل عليه بعد اول Setting For this Value
//}


using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOS.BasketDTO
{
    public record BasketItemDto
    {
        public int Id { get; init; }

        public string ProductName { get; init; } = string.Empty;

        public string PictureUrl { get; init; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; init; }

        [Range(1, 100)]
        public int Quantity { get; init; }
    }
}
