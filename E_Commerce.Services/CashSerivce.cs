using E_Commerce.Domain.Constracts;
using E_Commerce.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CashSerivce : ICashService
    {
        private readonly ICashRepository _cashRepository;
        //To Ask CLr tto inject Object from ICashRepository=>روح علمه فى الMain Function
        public CashSerivce(ICashRepository cashRepository)
        {
            _cashRepository = cashRepository;
        }
        public async Task<string?> GetAsync(string CashKey) => await _cashRepository.GetAsync(CashKey);//this Get CashValue by CashKey
        public async Task SetAsync(string CashKey, object CashValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(CashValue,new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase,
            });
            await _cashRepository.SetAsync(CashKey, value, TimeToLive);
        }
    }
}
