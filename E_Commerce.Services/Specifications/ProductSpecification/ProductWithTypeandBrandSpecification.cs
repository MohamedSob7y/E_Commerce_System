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
        #region For GetTypeByIdWithInclude And GetBrandByIdWithIncludes and Search And Sorting
        //this Constructor With three Cases 
        public ProductWithTypeandBrandSpecification(ProductQueryParam productQuery)
        : base(P => 
        (!productQuery.TypeId.HasValue || P.ProductTypeId == productQuery.TypeId.Value)&&
        (!productQuery.BrandId.HasValue || P.ProductBrandId == productQuery.BrandId.Value)&&
        (string.IsNullOrEmpty(productQuery.Search) ||P.Name.ToLower().Contains(productQuery.Search.ToLower())))  
            //this Just For Brand and Type With includes
            //This Can use With Brand Only with includes
            //This Can Use With Type Only with  includes
            //This Can Use With Search Only or Search with Type only or Search with Type and brand  or Search With Brand Only
        {
            //For Chain Base Constrauctor 
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch(productQuery.Sort)
            {
                case ProductsortingOptions.NameAsc:
                    AddOrderBy(P=>P.Name);
                    break;
                case ProductsortingOptions.NameDesc:
                    AddOrderByDescening(P=>P.Name);
                    break;
                    case ProductsortingOptions.PriceAsc:
                    AddOrderBy(P=>P.Price);
                    break;
                case ProductsortingOptions.PriceDesc:
                    AddOrderByDescening(P => P.Price);
                    break;
                default:
                    //المفروض ان default انه بيرتب based on id Ascending
                    //طب ليه اعمل default وكدة كدة اصلا هيتربت بناء على id Ascending 
                    //لان ممكن اشيل الPrimary Key من عليه اخليه على حاجة تانيه  يعنى اخلى Primary Key on Anthore Clustred index مختلف عن الId فكدة غير فى الLogic  اللى انا عايزه ودا غلط 
                    //الـdefault سيتم ترتيبه حسب الـId تصاعديًا بشكل تلقائي، لكنني أضيف default تحسبًا لتغيير الـPrimary Key أو الـClustered Index.
                    AddOrderBy(P => P.Id);
                    break;
            }
            ApplyPagination
          (productQuery.PageSize, productQuery.PageIndex);
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
        
    }
}
