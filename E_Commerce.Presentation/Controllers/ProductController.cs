using E_Commerce.Services.Abstraction;
using E_Commerce.Shared.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
           _productService = productService;
        }
        #region All Endpiont
        [HttpGet]
        //Url:{{baseUrl}}/api/nameofController/NameofEndpoint
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            //لازم اعرف Swagger اية هى نوع الData اللى هترجع لما يعمل Test بالتالى لازم اعرفه 
            var Products =await _productService.GetAllProductsAsync();
            return Ok(Products);
        }

        [HttpGet("{id}")]
        //{{baseUrl}}/api/Product/GetProductById/2    كدة انا بعت Id بطريقة الRoutes
        //طريقة الquery Paramer {{baseUrl}}/api/Product/GetProductById?id=2
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product=await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        [HttpGet("brands")]
        //{{baseUrl}}/api/Product/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrand()
        {
            var Brands=await _productService.GetAllBrandAsync();
            return Ok(Brands);
        }
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
