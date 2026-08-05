using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Middleware
{
    //عشان الclassnدا يتفهم انه Middlware    
    //First implement interface IMiddleware
    //or Make Constructor take RequestDelegate يقدر من خلالها يوصل للnext Middleware 
    //2 حاجة يكون عنده method invoke call NextMiddlware by using  Take HttpContext
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        //this LoggerFromMicrosoft بستخدمه عشان احدد بالتفصيل اى الErrors مش بس فى الConsole انا كمان عايز اعرضها فى الJson+ Application 
        //وكدة كدة مش لازم اعرف الClr هى معمولة اصلا 
        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _Logger = logger;
        }

        public ILogger<ExceptionHandlerMiddleware> _Logger { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);//Call Next Middlware by using Context دا فى حالة ان محصلشى اى مشكلة فى تنفيذ الmiddlware
                //Context دا معناها انا بتحكم فى الrequest والFlow بتاعه كامل 
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex,"something Went Wrong");//this Log in Console بس طبعا بعد كدة بنعمل File Logs عشان لما اعمل Maintance واشوف الغلطات اللى موجودة والBuggs
                //====================================================================================
                 //in Json بقا يظهر اية الStatus + Error وتفاصيل المشكلة 
                context.Response.StatusCode=StatusCodes.Status500InternalServerError;//هنا بتحكم فى Status code of Network 
                var problem = new ProblemDetails()
                {
                    Title ="An unExpected Error occured",
                    Status=StatusCodes.Status500InternalServerError,
                    //بتحمكم فى الStatusCode of Message اللى بتتعرض جوه الJson
                    Detail=ex.Message,
                    Instance=context.Request.Path,
                    //هنا بقا بعرفه مين الrequest اللى عمل المشكلة اصلا 
                };
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
