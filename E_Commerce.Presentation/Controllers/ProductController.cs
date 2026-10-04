using E_Commerce.Presentation.Attributes;
using E_Commerce.Presentation.Controllers;
using E_Commerce.Services.Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Api.Controllers
{
    
    public class ProductController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
           _productService = productService;
        }
        #region All Endpiont
        //=========================================================
        [RedisCash(10)]//وانا ببعت للRedis Cash ابعتله الDurationيعنى مدة بقاء الdata innCash 
        //After Making Attribute For Cashing 
        //كدة انا طبقت فكرة الcash => if Data Exsist in Cash git it Without Call Service and Database 
        //if Data مش موجودة فى ال cash خلاص كمل الFlow بتاعك عادى Send To Service then To Database 
        [HttpGet]
        //Url:{{baseUrl}}/api/nameofController/NameofEndpoint
        #region Before Specification Design Pattern For GetByIdBrandType
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        //{
        //    //لازم اعرف Swagger اية هى نوع الData اللى هترجع لما يعمل Test بالتالى لازم اعرفه 
        //    var Products = await _productService.GetAllProductsAsync();
        //    return Ok(Products);
        //}

        #endregion
        //=========================================================
        #region After Specification Design Pattern For GetByIdBrandType and Search
        //دى بعملها عشان وانا بعمل Get All Product ابعتله الBrandId+ TypeId عشان يعمل الFiletration بتاعهم 
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts
        //    ([FromQuery]ProductQueryParam productQueryParam)
        //{
        //    //لازم اعرف Swagger اية هى نوع الData اللى هترجع لما يعمل Test بالتالى لازم اعرفه 
        //    var Products = await _productService.GetAllProductsAsync(productQueryParam);
        //    return Ok(Products);
        //}

        #endregion
        //=========================================================
        #region After Make Pagination and Make Specificaiton Design Pattern
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts
            ([FromQuery] ProductQueryParam productQueryParam)
        {
            //لازم اعرف Swagger اية هى نوع الData اللى هترجع لما يعمل Test بالتالى لازم اعرفه 
            var Products = await _productService.GetAllProductsAsync(productQueryParam);
            return Ok(Products);
        }


        #endregion
        //=========================================================
        [HttpGet("{id}")]
        //{{baseUrl}}/api/Product/GetProductById/2    كدة انا بعت Id بطريقة الRoutes
        //طريقة الquery Paramer {{baseUrl}}/api/Product/GetProductById?id=2
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var Result=await _productService.GetProductByIdAsync(id);//this return Result<ProductDTO>
                                                                     //================================================
            #region Solving Problem With if Product is Not Exsist 
            //if Product is Not Exsist in Database=>  انا هنا مطالب انى اعمل الكلام دا فى كل حتة  لانى لسة هضيف الorder بالتالى هضطر اعمل نفس الكلام دا تانى فى Order Controller عشان كدة لازم اعمله فى مكان مستخدم لكل حاجة هى 
            //Controller وانا مش عايز اعمل اى لوجيك فى ال
            //لازم اللى بيعمل الكلام دا الservice مش الcontroller 
            //if (product is null)
            //{
            //    return NotFound(new ProblemDetails()
            //    {
            //        Title = "Product Not Found",
            //        Status = StatusCodes.Status404NotFound,
            //        Detail = $"Product with Id: {id} is not found",
            //        Instance = HttpContext.Request.Path
            //    });
            //} 
            #endregion
            //================================================
            #region After Making Result Pattern
            return Ok(Result);//if Success


            return new ProblemDetails//if Fail

            #endregion


        }
        //=========================================================
        [HttpGet("brands")]
        //{{baseUrl}}/api/Product/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrand()
        {
            var Brands=await _productService.GetAllBrandAsync();
            return Ok(Brands);
        }
        //=========================================================
        [HttpGet("types")]
        //{{baseUrl}}/api/Product/types    بدل ماكتب اسم الEndpiont اكتب Static Segment على طول بدل كدة لانه بيدور على كل الEndpiont اللى بيعملوا Get
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypeAsync();
            return Ok(Types);
        }
        #endregion
    }
}
