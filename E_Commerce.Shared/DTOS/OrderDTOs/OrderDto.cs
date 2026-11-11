using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.OrderDTOs
{
    public record OrderDto
    {
        public string BasketId { get; init; }
        public int DeliveryMethodId { get; init; } 
        
        public AddressDto Address { get; init; }

    }
}
