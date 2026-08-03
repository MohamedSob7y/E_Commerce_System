using E_Commerce.Domain.Constracts;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repositories
{
    public class CashRepository : ICashRepository
    {
        private readonly IDatabaseAsync _database;

        public CashRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string CashKey)//Red Data From Cash 
        {
            var cashvalue = await _database.StringGetAsync(CashKey);
            //if(cashvalue.IsNullOrEmpty)
            //{
            //    return null;
            //}
            //return cashvalue.ToString();
            //ليه عملتها Tostring عشان بترجعلى بFrom type Redis Value وانا عايزها string عشان كدة حولتها على طول الى tostring 
            return cashvalue.IsNullOrEmpty ? null : cashvalue.ToString();
        }
        //Put Data in Cash if Cash Not Contain Data
        public async Task SetAsync(string CashKey, string CashValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CashKey, CashValue, TimeToLive);//Put Data in Cash if Cash Not Contain Data
        }
    }
}
