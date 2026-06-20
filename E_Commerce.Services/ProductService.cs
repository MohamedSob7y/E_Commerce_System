using AutoMapper;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes;
using E_Commerce.Services.Abstraction;
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

        public ProductService(IUniteofWork uniteofWork,IMapper mapper)
        {
          _uniteofWork = uniteofWork;
            _Mapper = mapper;
        }

        public IMapper _Mapper { get; }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandAsync()
        {
            var Brands = await _uniteofWork.GetRepository<ProductBrand,int>().GetAllAsync();
            if (!Brands.Any() || Brands is null) return [];
            return _Mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products=await _uniteofWork.GetRepository<Product,int>().GetAllAsync();
            if (!products.Any() || products is null) return [];
            return _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypeAsync()
        {
            var Types = await _uniteofWork.GetRepository<ProductType, int>().GetAllAsync();
            if (!Types.Any() || Types is null) return [];
            return _Mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product=await _uniteofWork.GetRepository<Product,int>().GetByIdAsync(id);
            if (product is null) return null;
            return _Mapper.Map<Product, ProductDTO>(product);
        }
    }
}
