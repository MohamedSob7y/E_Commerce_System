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
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email);
        Task<Result<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync();
        //GetAllOrders For Specific User
        Task<Result<IEnumerable<OrderToReturnDto>>> GetAllOrdersAsync(string email);//take This Email From Token

        //Method Return Order By Id For Specific User => return Only one Order Not All Orders For this User يعنى بترجع Order واحد على حسب Id بتاعه للUser دا 
        Task<Result<OrderToReturnDto>> GetOrderByIdForUserAsync(Guid Id,string email);
    }
}
