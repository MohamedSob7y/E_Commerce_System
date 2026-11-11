using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.OrderDTOs
{
    public record OrderToReturnDto
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; }
        public ICollection<OrderItemDto> Items { get; init; }
        public AddressDto Address { get; init; } 
        public string DeliveryMethod { get; init; }
        public string Status { get; init; }
        public DateTimeOffset OrderDate { get; init; }
        public decimal Total { get; init; }
        public decimal SubTotal { get; init; }
    }
}
