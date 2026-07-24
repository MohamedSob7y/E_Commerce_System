using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes.Basket_Module;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDatabase _database;

        //مش هنيفع هنا استخدم الDbContext لانه بيقرا من Database and this Module مش متخزن فى الDatabase Just Stored i Memory using Redis [Cash  ]
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();//كدة خليته يشاور على Object دا لانى بتعامل مع Key + Value

            //inject Object From Any class implement interface IDistributedCache=> معناها بستخدمها وميهمنيش اى Technology دا مستخدم مع كله 

            //=========================================================================================================
            //So Ask Clr to inject object From any class implement interface  IConnectionMultiplexer  دى معناها ان بقؤل ان RepoBasketModule الData بتاعته تتخزن فى Redis No Sql DB in Memory Temporarly Not in Disk عشان كدة بستخدم الObject دا 
        }
        //===================================================================
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);//this Take Key and Will Return Value
            //Get Basket From Redis=> this return Redis Value عشان JsonObject بالتالى محتاج اعملها Deserializing
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            else
                //Make Deserialization عشان احوله الى object
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
        //===================================================================
        public async Task<CustomerBasket?> CreateorUpdateBasketAsync(CustomerBasket basket, TimeSpan TimeToLive = default)
        {
            //Convert Object To Json
            var JsonBasket = JsonSerializer.Serialize(basket);//Convert CustomerBasket To JsonString لانها تتضاف كKey+Value
            //====================================================================================================================
            //Save Key+Value in Redis => if Key مش موجود بيتعمل ولو موجود بيعمل override عليه 
            var iscreatedodupdated = await _database.StringSetAsync(basket.Id, JsonBasket, (TimeToLive == default) ? TimeSpan.FromDays(7) : TimeToLive);//Add Key+Value+TTL Of Items دا فى حاة انه مش موجود ولسة بضيفه 
            //لو مبعتش اى TimetoLive خلاص خلى الData in Database حوالى 7 ايام 

            //====================================================================================================================
            //Read data from redis after saving it then make Deserializong لان الobject اللى جاى من نوع Json فلازم احوله لString
           return  await GetBasketAsync(basket.Id);


        }
        //===================================================================
        public async Task<bool> DeletBasketAsync(string BasketId)
        {
            //Key is Basketid
            return await _database.KeyDeleteAsync(BasketId);
        }

       
    }
}
