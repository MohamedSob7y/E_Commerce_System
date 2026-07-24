using E_Commerce.Shared.DTOS.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Abstraction
{
    public interface IBasketService
    {
        Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basketDTO);
        Task<BasketDTO> GetBasketAsync(string basketid);
        Task<bool> DeleteBasketAsync(string basketid);
    }
}
