using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Attributes
{
    //لو عندى logic وعايز كل endpionts تستخدمه يبقى نفذ فكرة الMiddleware 
    //لو عندى logic وعايز تستخدمه Specific endpiont => use Attribute For Cash Call Filter and Action
    public class RedisCashAttribute:ActionFilterAttribute
    {
        //Constructor of Attribute Class => is Special Type of Constructor مش هنيفع اعمل فيه اى حاجة يعنى مش هينفع اعمل Ask CLR to inject Object From Any Service 
        //this Contrsuctor 
        public RedisCashAttribute()
        {
            
        }
        //Excute Logic After Endpiont شغالة Syncronous
        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    base.OnActionExecuted(context);
        //}
        //Excute Logic Before Endpiont شغالة Syncronous
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //}
        //دى شغالة asyncrouse  وتقدر تتحكم تنفيذ اللوجيك قبل endpiont or After Endpiont
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Steps  1:GetCashService from Dependency injection Container using Explicit Injection as Constructor   2:check if Data Exsist in Cash return data and Skip Excuting Endpipnt مش هروحلها اصلا  this else Send Request To Endpint Then Service and Store Result of Endpiont in Cash For Next Request  بس دا فى حالة ان Endpiont return ok يعنى نجحت غير كدا مش هخزن اى حاجة 
            return base.OnActionExecutionAsync(context, next);
            //Context يعنى ماسك الRequets اللى جايلى 
            //next => Action بعد تنفيذ الRequest اللى جايلى هنا 
            //يعنى انا ماسك الطلب وماسك اية اللى هيحصل بعد كدة 
        }
    }
}
