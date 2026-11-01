using E_Commerce.Shared.Common_Responses;
using E_Commerce.Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Abstraction
{
    public interface IOrderService
    {
         //CreateOrder(OrderDTO)=>return OrdertoreturnDto
         Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto,string email);
    }
}
