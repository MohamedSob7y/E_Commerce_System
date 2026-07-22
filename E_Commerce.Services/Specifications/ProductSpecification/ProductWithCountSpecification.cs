using E_Commerce.Domain.Entityes;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.ProductSpecification
{
    //this Class Just For Count Before MakePagination and Not make Any Filteration or make any includes
    public class ProductWithCountSpecification:BaseSpecification<Product,int>
    {
        //عشان كدة اى تعديل هنا فى Constructor دا لازم اعدل على الConstructor in Parent in ProductWithBrandandTypeSpecification
        //قصدى ان This Class and Class Product With Type And Brand Specificaton عندهم نفس الContrucotr دا عشان كدة لو فى اى تعديل على الConstructor دا لازم اعدل هناك فكدة انا بعدل فى مكانين عشان كدة هعمل Helper Class ولو فى اى تعديل كدة انا بعدل فى مكان واحد وانادى عليه هنا وهناك فى ProuctWithTypeAndBrandSpecification
        public ProductWithCountSpecification(ProductQueryParam queryParam)
            :base(ProductSpecificationHelper.GetCretaria(queryParam)) //this Class Send Cretaria To Base Specification
        {
            
        }
    }
}
