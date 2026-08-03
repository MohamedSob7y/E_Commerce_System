using E_Commerce.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly int _durationofCash;

        //Constructor of Attribute Class => is Special Type of Constructor مش هنيفع اعمل فيه اى حاجة يعنى مش هينفع اعمل Ask CLR to inject Object From Any Service 
        //this Contrsuctor 
        public RedisCashAttribute(int durationofCash=5)//كدة انا خليت اى حد يستخدم الredisCash يبعت Duration يعنى مدة بقاء الData in Cash ولو مبعتشى احط default
        {
            _durationofCash = durationofCash;
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
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Steps  1:GetCashService from Dependency injection Container using Explicit Injection as Constructor   2:check if Data Exsist in Cash return data and Skip Excuting Endpipnt مش هروحلها اصلا  this else Send Request To Endpint Then Service and Store Result of Endpiont in Cash For Next Request  بس دا فى حالة ان Endpiont return ok يعنى نجحت غير كدا مش هخزن اى حاجة 
            var cashservice = context.HttpContext.RequestServices.GetRequiredService<ICashService>();//بيشوف الrequest الحالى ويتاكد هل الobject اللى طالبه موجود ضمن Reqired sevices عشان يديله الobject دا بطريقة explicit not implict 
            //So Clr Search on DIContainer فيه كل Objects اللى امنت عايزها بطريقة Explicitly injection 
            //Context يعنى ماسك الRequets اللى جايلى 
            //RequestServices => this Conain All Service اللى موجودة فى DiContainer فانا بخلى الClr يدور على اللObject اللى انا عايزه من service جوه الDiConainer اللى انا ماسكه
           //===========================================================================================================================================================
            //Step02 Create CashKey based on Request Path and QueryParam   عشان انا عايز اطلع من request CashKey عشان اتاكد ان Key موجود فى الCash ولا لاء لو موجود رجع الValue ولو مش موجود خلاص put Data وكمل الFlow بتاعك عادى من غير مشاكل 
            //this By Using Cashservice عشان لما انادى جواها على Function Get دى بتاخد الKey عشان كدة لازم اكونه
            var cashkey=CreateCashKey(context.HttpContext.Request);
            //===========================================================================================================================================================
            //3: check if Data Exsist in Cash return data and Skip Excuting Endpipnt مش هروحلها اصلا  this else Send Request To Endpint Then Service and Store Result of Endpiont in Cash For Next Request  بس دا فى حالة ان Endpiont return ok يعنى نجحت غير كدا مش هخزن اى حاجة
            var cashvalue =await cashservice.GetAsync(cashkey);
            if(cashvalue is not null)
            {
                //لو الdata موجودة المفروض مش هيعمل excute For Endpiont ويروح يجيب الData From Database ولو 
                context.Result = new ContentResult()
                {
                    Content = cashvalue,
                    ContentType = "application/json",//this Type of Response is Json
                    StatusCode = StatusCodes.Status200OK,
                };
                //كل دا عشان اتحكم ال Response هيرجع ازاى وبتحكم فيه دا لو الData موجودة يرجعها على طول ويرجع نوع الResponse is Json + StatusCode is 200 OK
                return;
            }
            else
            {
               var ExcutedContext= await next.Invoke();//كدة نفذت الenpiont //invoke will excute Endpiont 
               //ExcutedContext دا يعتبر الVariable اللى شايل نتيجة تنفيذ الFlow of Endpoiont دا فى حالة انا data مش موجودة 
                if(ExcutedContext.Result is OkObjectResult result)
                {
                    await cashservice.SetAsync(cashkey, result.Value!, TimeSpan.FromMinutes(_durationofCash));
                }
               //لو مش موجودة يبقى لازم excute Endpiont then Store Data اللى راجعةمن Database in Cash For Next Request بس هخزن الData اللى راجعة فى حالة ان Response 200 is ok
            }
            //next => Action بعد تنفيذ الRequest اللى جايلى هنا 
            //يعنى انا ماسك الطلب وماسك اية اللى هيحصل بعد كدة 
        }

        //CashKey ممكن يكون  api/Product  
        //api/product?brandid=2
        //api/Products?typeid=1&&brandId=2
        //api/products/typeid=1
        //كل دول احتمالية Key اللى عندى للendpiont دى فقط اللى هى GetAllProducts
        private string CreateCashKey(HttpRequest request)
        {
            //لازم اعمل CashKey based on Request اللى جايلى 
            StringBuilder key=new StringBuilder();//use string builder عشان هو mutable لان لو استخدمت الstring is immutable يعنى مش هعرف اعدل على Key 
            //عشان كدة استخمدت ال string builder عشان اعرف اعدل على الKey براحتى بقا 
            key.Append(request.Path);//api/products
            foreach(var item in request.Query.OrderBy(x=>x.Key))
            {
                key.Append($"{item.Key}-{item.Value}"); //api/product?brandid=2 بيحط عليها بقا الQuery Param عشان هو اللى متغير انما انا ثبت الapi/product/دا اللى متغير وباخده الى QueryParam
            }
            return key.ToString();
        }
    }
}
