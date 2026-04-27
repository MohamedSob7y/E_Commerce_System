using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entityes
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; } = default!;//default =null  ! is null Forgiven Operator
        public string Description { get; set; } = null!;
        public string PictureURL { get; set; } = null!;
        public decimal? Price { get; set; }
        //===================================================
        #region Relation
        public ProductBrand ProductBrand { get; set; } = null!;//is Mandatory
        public int ProductBrandId {  get; set; }
        public ProductType ProductType { get; set; } = null!;
        public int ProductTypeId { get; set; }

        #endregion

    }
}
