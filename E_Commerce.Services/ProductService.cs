using AutoMapper;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Services.Abstraction;
using E_Commerce.Services.Exceptions;
using E_Commerce.Services.Specifications.ProductSpecification;
using E_Commerce.Shared;
using E_Commerce.Shared.Common_Responses;
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
            var Repo = _uniteofWork.GetRepository<Product, int>();
            //دى عملتها كدة عشان عايز اعمل Navigation Property تكون Loaded عندى وانا بعمل GetAll Product=> تظهر معايا ال ProductBrand + ProductTypes 
            //عايز اكون specification object For include Navigation Property => ProductBrand + ProductTypes
            //=================================================================================================================================
            var Spec = new ProductWithTypeandBrandSpecification(productQueryParam);
            var products = await Repo.GetAllAsync(Spec);
            //=================================================================================================================================
            var newSpec = new ProductWithCountSpecification(productQueryParam);
            var totalCount = await Repo.CountAsync(newSpec);//this For Count Before Pagination عشان كدة عملت New specification جديدة لان لو بعتلها القديمة اللى هى ProductWithTypeAndBrandSpecification كدة هو هيعمل الCount After Pagination وانا مش عايز كدة عشان كدة عملت واحدة جديدة للحتة دى  
            //=================================================================================================================================
            var DataToReturn = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
            var CountofReturnedData = DataToReturn.Count();
            //=================================================================================================================================
            return new PaginatedResult<ProductDTO>
                (productQueryParam.PageIndex, CountofReturnedData, totalCount, DataToReturn);
        }
        #endregion
        //=============================================================
        #region Before Result Pattern
        //public async Task<ProductDTO> GetProductByIdAsync(int id)
        //{
        //    #region Before Specification Design Pattern
        //    //var product = await _uniteofWork.GetRepository<Product, int>().GetByIdAsync(id);
        //    //if (product is null) return null;
        //    //return _Mapper.Map<Product, ProductDTO>(product);
        //    #endregion
        //    //=============================================================
        //    #region After Specification Design Pattern
        //    var spec = new ProductWithTypeandBrandSpecification(id);
        //    var product = await _uniteofWork.GetRepository<Product, int>().GetByIdAsync(spec);
        //    //=========================================
        //    #region Throw Exception
        //    if (product is null)
        //        throw new ProductNotFoundException(id);
        //    #endregion
        //    //=========================================
        //    return _Mapper.Map<Product, ProductDTO>(product);
        //    #endregion
        //} 
        #endregion
        //=============================================================
        #region After Result Pattern
        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
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
            //=========================================
            #region Throw Exception Before Result Pattern
            //if (product is null)
            //    throw new ProductNotFoundException(id);
            #endregion
            //=========================================
            #region After Result Pattern After Operator Overloading
            if (product is null) 
                return Error.NotFound("Product Not Found", $"Product with this id : {id} is Not Found");//this Single Error
            //this Genrate object From Result<ProductDTO> using Method Fail وباعتلها error نوعه NoFound بدل ماكل شوية انادى عليها بالطريقة كاملة 
            #endregion
            //=========================================
            //using Static Factory Method After Makeing Operator Overloading in Result Class
            return _Mapper.Map<Product, ProductDTO>(product);//this create Object from REsult<ProductDTO> using Method Ok That Take Value

            //عشان كدة بدل ماعمل الobject وانادي على Function اللى بياخدها لاء انا عامل Operator Overloading تبعت الData على طول فى حالة الSuccess using Ok يعنى return Data فقط ودى معناها انك عملت object fRom Result<ProductDTO> using Ok That Take Data
            //بدل ماعمل object وانادى على Method Fail واابعتلها الErrr => لاء انا هبعت الerror على طول يعنى هعمل return For Error Direct that means that You Create object from Result<ProductDto>  using Method Fail That Take Error يعنى انا بعمل return على طول للحاجة اللى بتاخدها الStatic Factory Method like Data or Error or List Of Error => using Operator Overloading 
           
            //Before Operator Overloading
            //return Result<ProductDTO>.Ok(_Mapper.Map<Product, ProductDTO>(product));


            #endregion
        }
        #endregion
        //=============================================================
        public async Task<IEnumerable<TypeDTO>> GetAllTypeAsync()
        {
            var Types = await _uniteofWork.GetRepository<ProductType, int>().GetAllAsync();
            if (!Types.Any() || Types is null) return [];
            return _Mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDTO>>(Types);
        }


    }
}
