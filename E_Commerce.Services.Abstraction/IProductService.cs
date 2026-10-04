using E_Commerce.Shared;
using E_Commerce.Shared.Common_Responses;
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
        #region After Applying Pagination and After Applying Specification Design Pattern
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParam productQueryParam);
        #endregion
        //=================================================
        #region Before Result Pattern
        //Task<ProductDTO> GetProductByIdAsync(int id);
        #endregion
        //=================================================
        #region After Result Pattern
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        #endregion
        //=================================================
        Task<IEnumerable<BrandDTO>> GetAllBrandAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypeAsync();
    }
}
