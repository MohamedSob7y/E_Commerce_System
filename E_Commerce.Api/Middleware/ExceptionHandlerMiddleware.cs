using E_Commerce.Services.Exceptions;
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

        #region For General Exception
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    try
        //    {
        //        await _next.Invoke(context);//Call Next Middlware by using Context دا فى حالة ان محصلشى اى مشكلة فى تنفيذ الmiddlware
        //                                    //Context دا معناها انا بتحكم فى الrequest والFlow بتاعه كامل 

        //        #region For 404_URL_NotFound Exception
        //        //دى حل لمشكلة الURL Not Found 
        //        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        //        {
        //            var problem = new ProblemDetails()
        //            {
        //                Title = "URL Not Found",
        //                Status = StatusCodes.Status404NotFound,
        //                Detail = $"Endpiont: {context.Request.Path} is not found",
        //                Instance = context.Request.Path,//this REquest اللى سبب المشكلة اصلا 
        //            };
        //            await context.Response.WriteAsJsonAsync(problem);
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        _Logger.LogError(ex, "something Went Wrong");//this Log in Console بس طبعا بعد كدة بنعمل File Logs عشان لما اعمل Maintance واشوف الغلطات اللى موجودة والBuggs
        //        //====================================================================================
        //       //in Json بقا يظهر اية الStatus + Error وتفاصيل المشكلة 
        //        //context.Response.StatusCode = StatusCodes.Status500InternalServerError;//هنا بتحكم فى Status code of Network 
        //        var problem = new ProblemDetails()
        //        {
        //            Title = "An unExpected Error occured",
        //            //بتحمكم فى الStatusCode of Message اللى بتتعرض جوه الJson
        //            Detail = ex.Message,
        //            Instance = context.Request.Path,
        //            //هنا بقا بعرفه مين الrequest اللى عمل المشكلة اصلا 

        //            //this Strategy Pattern ان على حسب الexception اللى راجع اطبع الStatus Code بتاعه 
        //            //عايز بقا على حسب نوع الException اعرض الStatus Code الخاص بيها 
        //            Status = ex switch
        //            {
        //                NotFoundException=>StatusCodes.Status404NotFound,
        //                _ => StatusCodes.Status500InternalServerError //Else دى زى 
        //            },

        //        };
        //        context.Response.StatusCode = problem.Status.Value;  //كدة انا غيرت فى الStatus Code of Network خلتها زى الStatus Code in Json
        //        await context.Response.WriteAsJsonAsync(problem);
        //    }
        //} 



        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle 404 only when no response body has started
                if (context.Response.StatusCode ==
                        StatusCodes.Status404NotFound
                    && !context.Response.HasStarted
                    /*
                     
                     */
                    && context.GetEndpoint() == null)
                {
                    var problem = new ProblemDetails
                    {
                        Title = "URL Not Found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = $"Endpoint: {context.Request.Path} is not found",
                        Instance = context.Request.Path
                    };

                    context.Response.ContentType = "application/problem+json";

                    await context.Response.WriteAsJsonAsync(problem);
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Something Went Wrong");

                // Cannot change the response after it has started
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();

                var statusCode = ex switch
                {
                    NotFoundException =>
                        StatusCodes.Status404NotFound,

                    _ =>
                        StatusCodes.Status500InternalServerError
                };

                var problem = new ProblemDetails
                {
                    Title = statusCode == StatusCodes.Status404NotFound
                        ? "Resource Not Found"
                        : "An Unexpected Error Occurred",

                    Status = statusCode,
                    Detail = context.RequestServices
                        .GetRequiredService<IHostEnvironment>()
                        .IsDevelopment()
                            ? ex.Message
                            : "An error occurred while processing the request.",

                    Instance = context.Request.Path
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
        #endregion
    }
}
