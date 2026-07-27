using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entityes.Basket_Module
{
    //this Class مش هيورث من BaseEntity لان انا قؤلت ان اى Class Will Inherit From BaseEntity هيتحول الى Table in Database 
    //ودا اصلا مش هيتحول الى Table in Database عشان كدة مش هخليه يورث من Base Entityt
    public class CustomerBasket
    {
        //public string Id { get; set; } = default!;
        //Created From Frontend using Guid
        //ICollection<BasketItem> Items { get; set; } = [];
       

        public string Id { get; set; } = string.Empty;
        //Created From Frontend using Guid
        public ICollection<BasketItem> Items { get; set; }
            = new List<BasketItem>();
        //كل Basket Contain Item
        //All Cycle 1: AddProduct To Basket   2:then Make All Items in Basket For Orders    3:then make Payment for All Orders
        
    }
}
