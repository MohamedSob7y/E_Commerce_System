using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared
{
    public class ProductQueryParam
    {
        public int? BrandId {  get; set; }
        public int? TypeId { get; set; }
        public string? Search {  get; set; }
        public ProductsortingOptions Sort { get; set; } //خلتها enums لانى عارف القيم 
        //take PageSize+Pageindex بس لازم اعمل Validation عشان دى قيم باخدها من Frontend 
       
        private int _PageIndex = 1;//by default عشان لو Frontend مبعتلش اخليه يجيب اول خمسة Product 

        public int PageIndex
        {
            get { return _PageIndex; }
            set 
            {
                _PageIndex= (value<=0)?1:value;//يعنى لو باعت رقم صفحة اصغر من 0 كدة مش باعت اصلا بالتالى اخليها 1 غير كدة خد القيمة الصحيحة  
            }//must make Validation For Value That Sent From Frontend 
        }

        private const int _DefaultPageSize = 5;
        private int _PageSize= _DefaultPageSize;
        private const int _MaxPageSize = 10;
        public int PageSize
        {
            get { return _PageSize; }
            set 
            {
                if (value <= 0)
                {
                    _PageSize = _DefaultPageSize;
                }
                else if (value >= 10)
                {
                    _PageSize = _MaxPageSize;
                }
                else
                {
                    _PageSize = value;
                }
            }
        }


    }
}
