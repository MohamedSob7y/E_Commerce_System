using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entityes.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        //بمجرد ماعمل الObject from Order in application ويتعمل الtable دا فى الDatabase => Get CurrentTime يعنى يجيب الوقت الحالى بمجرد مايتعمل عشان يبدا يشتغل 
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress Address { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }//this FK as Delivery Method has Mutli Orders and Order has Only one Delivery Method
        public ICollection<OrderItems> Items { get; set; } = [];
        public decimal SubTotal { get; set; }//Total Price of Items QuantityFor Each Item * Price For Each uantity
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;//Subtotal+DeliveryCost=> when Using AutoMapper هيفهم من دى انها readonly Property لانها Get+ اسم الProperty ويهحاول يحلها فى الRuntime تعتبر هى AutoCalculated
    }
}
