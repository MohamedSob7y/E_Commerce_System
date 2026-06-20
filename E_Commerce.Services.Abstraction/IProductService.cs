using E_Commerce.Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Abstraction
{
    public interface IProductService
    {
       Task< IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypeAsync();
    }
}
