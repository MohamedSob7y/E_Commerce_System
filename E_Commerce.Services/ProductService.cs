using AutoMapper;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Services.Abstraction;
using E_Commerce.Services.Specifications.ProductSpecification;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUniteofWork _uniteofWork;

        public ProductService(IUniteofWork uniteofWork, IMapper mapper)
        {
            _uniteofWork = uniteofWork;
            _Mapper = mapper;
        }

        public IMapper _Mapper { get; }
        //=============================================================
        public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
        {
            var Brands = await _uniteofWork.GetRepository<ProductBrand, int>().GetAllAsync();
            if (!Brands.Any() || Brands is null) return [];
            return _Mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDTO>>(Brands);
        }

        //=============================================================
        #region For GetAllProductWithIncludes
        //public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()//For Filteration For Brand+Type
        //{
        //    #region Before Specification Design Pattern 
        //    //var products = await _uniteofWork.GetRepository<Product, int>().GetAllAsync();
        //    //if (!products.Any() || products is null) return [];
        //    //return _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products); 
        //    #endregion
        //    //=======================================
        //    #region After Specification Design Pattern
        //    //دى عملتها كدة عشان عايز اعمل Navigation Property تكون Loaded عندى وانا بعمل GetAll Product=> تظهر معايا ال ProductBrand + ProductTypes 
        //    //عايز اكون specification object For include Navigation Property => ProductBrand + ProductTypes
        //    var Spec = new ProductWithTypeandBrandSpecification();
        //    var products = await _uniteofWork.GetRepository<Product, int>().GetAllAsync(Spec);
        //    if (!products.Any() || products is null) return [];
        //    return _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

        //    #endregion
        //}

        #endregion
        //=============================================================
        #region GetbrandbyidWithInclude+GetTypebyid with Include
        //public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductQueryParam productQueryParam)//For Filteration For Brand+Type
        //{
        //    //دى عملتها كدة عشان عايز اعمل Navigation Property تكون Loaded عندى وانا بعمل GetAll Product=> تظهر معايا ال ProductBrand + ProductTypes 
        //    //عايز اكون specification object For include Navigation Property => ProductBrand + ProductTypes
        //    var Spec = new ProductWithTypeandBrandSpecification(productQueryParam);
        //    var products = await _uniteofWork.GetRepository<Product, int>().GetAllAsync(Spec);
        //    if (!products.Any() || products is null) return [];
        //    return _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
        //}
        #endregion
        //=============================================================
        #region GetAll Product After Applying Pagination
        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParam productQueryParam)//For Filteration For Brand+Type
        {
            var Repo =  _uniteofWork.GetRepository<Product, int>();
            //دى عملتها كدة عشان عايز اعمل Navigation Property تكون Loaded عندى وانا بعمل GetAll Product=> تظهر معايا ال ProductBrand + ProductTypes 
            //عايز اكون specification object For include Navigation Property => ProductBrand + ProductTypes
            //=================================================================================================================================
            var Spec = new ProductWithTypeandBrandSpecification(productQueryParam);
            var products =await Repo.GetAllAsync(Spec);
            //=================================================================================================================================
            var newSpec =new ProductWithCountSpecification(productQueryParam);
            var totalCount=await Repo.CountAsync(newSpec);//this For Count Before Pagination 
            //=================================================================================================================================
            var DataToReturn = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            var CountofReturnedData = DataToReturn.Count();
            //=================================================================================================================================
            return new PaginatedResult<ProductDTO>
                (productQueryParam.PageIndex, CountofReturnedData, totalCount, DataToReturn);
        }
        #endregion
        //=============================================================
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            #region Before Specification Design Pattern
            //var product = await _uniteofWork.GetRepository<Product, int>().GetByIdAsync(id);
            //if (product is null) return null;
            //return _Mapper.Map<Product, ProductDTO>(product);
            #endregion
            //=============================================================
            #region After Specification Design Pattern
            var spec = new ProductWithTypeandBrandSpecification(id);
            var product = await _uniteofWork.GetRepository<Product, int>().GetByIdAsync(spec);
            if (product is null) return null;
            return _Mapper.Map<Product, ProductDTO>(product);
            #endregion
        }
        //=============================================================
        public async Task<IEnumerable<TypeDTO>> GetAllTypeAsync()
        {
            var Types = await _uniteofWork.GetRepository<ProductType, int>().GetAllAsync();
            if (!Types.Any() || Types is null) return [];
            return _Mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDTO>>(Types);
        }


    }
}
