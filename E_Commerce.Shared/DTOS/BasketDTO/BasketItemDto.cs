using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.BasketDTO
{
    public record BasketItemDto
        (int Id,
        string ProductName, 
        string PictureUrl,
        [Range(0,double.MaxValue)]
        decimal Price,
        [Range(0,100)]
         int Quantity
        );
    //So When Createing Object From This Type الValues اللى جواه هتكون Immutable يعنى مش هعرف اعدل عليه بعد اول Setting For this Value
}
