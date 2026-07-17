    using E_Commerce.Domain.Entityes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace E_Commerce.Services.Specifications.ProductSpecification
    {
        public class ProductWithTypeandBrandSpecification:BaseSpecification<Product,int>
        {
            public ProductWithTypeandBrandSpecification()
                :base()
            {
                //For Chain Base Constrauctor 
                AddInclude(p => p.ProductBrand);
                AddInclude(p => p.ProductType);
            }
        }
    }
