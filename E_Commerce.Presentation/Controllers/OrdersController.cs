using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.DTOS.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        [Authorize]
        //post BaseUrl/api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            //Take User Email From Token

            var result = await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return HandleResult(result);
        }

        [HttpGet]
        [Authorize]
        //Get Baseurl/api/Orders
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrders()
        {
            //Get Email From Token 

            var result = await _orderService.GetAllOrdersAsync(GetEmailFromToken());
            return HandleResult(result);
        }

        [Authorize] //عشان اخد منه Email From Token
        [HttpGet("{id:guid}")]
        //get BaseUrl/api/Orders => this Endpiont Take Id
        public async Task<ActionResult<OrderToReturnDto>> GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdForUserAsync(id, GetEmailFromToken());
            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("deliverymethods")]
        //Get BaseUrl/api/Orders/deliverymethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var result =await _orderService.GetAllDeliveryMethodsAsync();
            return HandleResult(result);
        }
        //اى endpiont بتاخد data From Token يبقى لازم بيتعمل calling For Service Take Data From Token After Login فلازم الendpiont دى تبقى Authorize
    }
}
