using AutoMapper;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes.Basket_Module;
using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.DTOS.BasketDTO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        //ask CLr To Inject object From IBasketRepository in Main.Cs
        public BasketService(
            IBasketRepository basketRepository,
            IMapper mapper
            )
        {
            _basketRepository = basketRepository;
            _Mapper = mapper;
        }

        public IMapper _Mapper { get; }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basketDTO)
        {
            //Convert DTO to CustomerBasket عشان اضيفه للCreateRepository وتاخد الCustomerBasket then Convert This To BasketDto
            //So Using Automatic Mapper
            var CustomerBasket = _Mapper.Map<BasketDTO, CustomerBasket>(basketDTO);
            var CreatedorUpdatedBasket = await _basketRepository.CreateorUpdateBasketAsync(CustomerBasket);
            return _Mapper.Map<BasketDTO>(CustomerBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketid) => await _basketRepository.DeletBasketAsync(basketid);


        public async Task<BasketDTO> GetBasketAsync(string basketid)
        {
            var basket=await _basketRepository.GetBasketAsync(basketid);
            return _Mapper.Map<BasketDTO>(basket);//Convert This To BasketDTO   
        }
    }
}
