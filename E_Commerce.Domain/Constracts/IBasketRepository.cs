using E_Commerce.Domain.Entityes.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    public interface IBasketRepository
    {
        //this interface contain Function Get Basket=>take Basketid then return Customer Basket ==Items 
        //Function Create or Update => if Item موجود فى الBasket update TTL  if Not Exsist Create it in Basket
        //Function Delete =>Take Basket Id then Delete Customer Basket
        Task<CustomerBasket?> GetBasketAsync(string BasketId);//return All CustomerBasket
        Task<CustomerBasket?> CreateorUpdateBasketAsync(CustomerBasket basket,TimeSpan TimeToLive=default);
        Task<bool>DeletBasketAsync(string BasketId);
    }
}
