using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.BasketDTO
{
    public record BasketDTO(string Id, ICollection<BasketItemDto> Items);
   //this DTO From Type Record Not Class => ليه عشان When Creating Object From This Type Call Record الValue اللى جواه هتكون Immutable يعنى مش هتقدر تعدل عليها بمجرد ماخدت اول Set ليها
   //عشان كدة عملتها Record من نوع  
}
