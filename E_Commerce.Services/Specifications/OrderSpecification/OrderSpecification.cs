using E_Commerce.Domain.Entityes.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.OrderSpecification
{
    public class OrderSpecification:BaseSpecification<Order,Guid>
    {
        public OrderSpecification(string email)
            :base(o=>o.UserEmail==email)
        {
            //Include DeliveryMethods + OrderItems
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            //لما يجى يعرض الOrders بيعرض الاحدث 
            AddOrderByDescening(o => o.OrderDate);
        }
        //Make Query For method GetOrderByIdForUserAsync => Filteration by Id + Email + Include item + include Delivery Method
        public OrderSpecification(Guid Id ,string email)
           : base(o => o.UserEmail == email&&o.Id==Id)
        {
            //Include DeliveryMethods + OrderItems
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
           
        }
    }
}
