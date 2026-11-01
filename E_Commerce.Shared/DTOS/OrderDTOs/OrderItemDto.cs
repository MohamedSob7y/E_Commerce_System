using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOS.OrderDTOs
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = default!;
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }= default!;
        public int Quantity { get; set; }

    }
}
