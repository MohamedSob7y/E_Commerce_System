using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Constracts
{
    public interface ICashRepository
    {
        //this Repository عشان تستخدمه الCashService ثم استخدم Cash Service in RedisCashAttribute Class in Presentation Layer
        //يعنى انا دلؤقتى عملت  RedisCashAttribute in Presentation Layer  المفروض هعمل Explicit injection For Cash Service عشان استخدمه جواه
        //then Make Cash Service Implementation عشان هستخدم الclass دا فى الCash attribute 
        //make CashRepository عشان هستخدمها فى Cash Service 
        Task<string?> GetAsync(string CashKey);//Take CashKey then return CashValue
        //this Method Get Data From Cash By Key if Key Exsist in Cash return Data and Value if Not Exsist put data 

        //============================================================================================================================

        //Function Put Data in Cash فى حالة ان الCash لا يحتوى على اى Data 
        Task SetAsync(string CashKey, string CashValue, TimeSpan TimeToLive);//Take CashKey and CashValue and ExpirationTime then Put Data in Cash    
    }
}
