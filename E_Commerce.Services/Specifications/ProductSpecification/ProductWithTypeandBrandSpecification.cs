    using E_Commerce.Domain.Entityes;
using E_Commerce.Shared;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace E_Commerce.Services.Specifications.ProductSpecification
    {
    public class ProductWithTypeandBrandSpecification : BaseSpecification<Product, int>
    {
        #region Just For GetAllProductWithIncludes
        //public ProductWithTypeandBrandSpecification()
        //: base(null)
        //{
        //    //For Chain Base Constrauctor 
        //    AddInclude(p => p.ProductBrand);
        //    AddInclude(p => p.ProductType);
        //}//this For GetAll includes only 
        #endregion
        //=========================================================================
        #region For GetTypeByIdWithInclude And GetBrandByIdWithIncludes
        //this Constructor With three Cases 
        public ProductWithTypeandBrandSpecification(ProductQueryParam productQuery)
        : base(P => 
        (!productQuery.TypeId.HasValue || P.ProductTypeId == productQuery.TypeId.Value)&&
        (!productQuery.BrandId.HasValue || P.ProductBrandId == productQuery.BrandId.Value)&&
        (string.IsNullOrEmpty(productQuery.Search) ||P.Name.ToLower().Contains(productQuery.Search.ToLower())))  
            //this Just For Brand and Type With includes
            //This Can use With Brand Only with includes
            //This Can Use With Type Only with  includes
        {
            //For Chain Base Constrauctor 
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }   
        #endregion
        //=========================================================================
        #region For GetProductByIdWith Includes
        //this Constructor For GetById for Filteration + Includes
        public ProductWithTypeandBrandSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
        #endregion
        //=========================================================================
    }
}
