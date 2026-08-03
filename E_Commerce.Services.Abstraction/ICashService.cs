using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Abstraction
{
    public interface ICashService
    {
        Task<string?> GetAsync(string CashKey);
        Task SetAsync(string CashKey, object CashValue, TimeSpan TimeToLive);//this Function Take Value as Json لانه جايلى من نوع Json 
        //عشان كدة انا عملت CashValue من نوع object لان الobject اللى جايلى معرفش نوعه اية وبعدين بعمل Deserialize عشان احوله 

    }
}
