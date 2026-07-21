using E_Commerce.Shared;
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
        #region Before Applying Pagination but After Applying Specification Design Pattern
        //Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductQueryParam productQueryParam);
        #endregion
        //=================================================
        #region Before Applying Pagination but After Applying Specification Design Pattern
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParam productQueryParam);
        #endregion
        //=================================================
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypeAsync();
    }
}
