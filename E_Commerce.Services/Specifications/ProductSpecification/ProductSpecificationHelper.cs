using E_Commerce.Domain.Entityes;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.ProductSpecification
{
    public static class ProductSpecificationHelper
    {
        //كدة اى تعديل بعمله هنا 
        //من غير ماعدل على ProductWithTypeAndBrandSpecification 
        //وكمان من غير ماعدل على Product With CountSpecification
        //this Class Send Cretaria To ProductWithTypeAndBrandSpecification  and Product With CountSpecification
        public static Expression<Func<Product, bool>> GetCretaria(ProductQueryParam queryParam)
        {
            return P =>
        (!queryParam.TypeId.HasValue || P.ProductTypeId == queryParam.TypeId.Value) &&
        (!queryParam.BrandId.HasValue || P.ProductBrandId == queryParam.BrandId.Value) &&
        (string.IsNullOrEmpty(queryParam.Search) || P.Name.ToLower().Contains(queryParam.Search.ToLower()));
            //this Just For Brand and Type With includes
            //This Can use With Brand Only with includes
            //This Can Use With Type Only with  includes
            //This Can Use With Search Only or Search with Type only or Search with Type and brand  or Search With Brand Only
        }
    }
}
