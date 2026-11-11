using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.OrderDTOs
{
    public record OrderItemDto
    {
        public string ProductName { get; init; } = default!;
        public decimal Price { get; init; }
        public string PictureUrl { get; init; }= default!;
        public int Quantity { get; init; }

    }
}
