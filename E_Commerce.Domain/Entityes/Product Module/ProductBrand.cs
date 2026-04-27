using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entityes
{
    public class ProductBrand:BaseEntity<int>
    {
        public string Name { get; set; } = null!;
    }
}
