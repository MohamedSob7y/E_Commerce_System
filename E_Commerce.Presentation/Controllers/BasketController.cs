using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.DTOS.BasketDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet("{basketId}")]
        public async Task<ActionResult<BasketDTO>> GetBasket(string basketid)
        {
            var basket = await _basketService.GetBasketAsync(basketid);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateorUpdateBasket(BasketDTO basketDTO)
        {
            var basket = await _basketService.CreateOrUpdateBasketAsync(basketDTO);
            return Ok(basket);
        }
        [HttpDelete("{basketId}")]//to take IdFrom Route
        public async Task<ActionResult<bool>>delete([FromRoute]string basketid)
        {
           var result= await _basketService.DeleteBasketAsync(basketid);
            return Ok(result);
        }
    }
}
