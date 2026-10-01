using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    //public class NotFoundException:Exception
    //{
    //    #region Classic Constructor 
    //    //public NotFoundException(string message)
    //    //   : base(message)
    //    //{
    //        //Base Class Will Sett Message in Exception Class
    //    //} //Call Parent Constructor to Set the Message in Exception Class
    //    #endregion
        
    //}
    //===============================
    //Anthor Solution with C#12
    #region Primary Constructor 
    public abstract class NotFoundException(string message) : Exception(message)
    {

    }

    //كدة انا اجبرت اى حد يعمل من Class دا object لازم يديله Parameter لان مش هينفع مدلوش لانه مش هيعرف هنا يعمل الEmpty ParameterLess Constructor 
    //وكمان بقيت ابعت الProperty اللى اسمها Message To Base Class [Excpetion] to Base Constructor to Setting Message in his Constructor
    //So This Property is immutable
    #endregion
    public sealed class  ProductNotFoundException(int id)
        : NotFoundException($"Product With Id : {id} is Not Found")
    {
        
    }

    public sealed class BasketNotFoundException(string id)
       : NotFoundException($"Basket With Id : {id} is Not Found")
    {

    }
}
