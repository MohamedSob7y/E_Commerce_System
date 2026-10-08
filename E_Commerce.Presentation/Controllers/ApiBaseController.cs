using E_Commerce.Shared.Common_Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        //this Class Handle Reult and Check For it then Send this To any class will inherit from ApiBaseController
        //كل دا بعمله عشان مش عايز الController بتاعه يعمل Check For Result Data اللى راجعه عشان مش عايزه يعمل اى logic
        //Flow Send Result to Product Controller then This Result Send to this Class to Checking For Result then Send it To Product Controller With Success Of Fail
        //===============================================================
        //===============================================================

        //Make Method That Handle Result اللى راجعة من Service واحدة راجعة with Data دى اللى هى   Generic والتانيه مش راجع بالData => Not Generic
        //===============================================================
        #region Handle Result without Data
        //Method return object From Result اللى راجع من Service Non Genrric يعنى بترجع بال object Without Data=> is Success return المفروض ارجع success return Success with no Content 204
        //if Result Failure => return Problem Deatils Along With Description + Status Code of this Problem
        protected IActionResult HandleResult(Result result)//Take Result Non Generic Object=> this Result جاية من Service  Check For This then Send It To Product Controller
        {
            if (result.IsSuccess)
            {
                return NoContent(); //كدة انا مخلتش يكون فى اى Logic in Product Controller لان بيورث من هنا واللوجيك كله هنا =>return 204 Ok With No Content
            }
            else //هنا بقا لو فىىمشكلة 
            {
                return HandleProblem(result.Errors);
            }
        }
        #endregion
        //===============================================================
        #region Handle Result With Data
        //Method اللى بتhandle object Result اللى راجع from Service بس راجع ومعاه Data is Generic فى بقا حالات 
        //Is Success return Success with Value 200 Ok
        //If Fail return Problem DEatils With Description + Status Code  
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);//return Result with Value 200
            }
            else //هنا بقا لو فىىمشكلة 
            {
                return HandleProblem(result.Errors);
            }
        }



        #endregion
        //===============================================================
        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            //if No Errors مش مبعوتة => return 500 لان مفيش اى error اتبعت بس فى مشكلة لان مش هينفع ابعت Ok 
            if (errors.Count == 0)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "an Error Occured"
                    );
            }
            //if has Multi Error but All Error are Validation Error=> return ValidationProblem object in Json
            if (errors.All(E => E.ErrorType == ErrorType.Validation))
            {
                return HandleValidationErrors(errors);
            }
            //Single Error اتبعت => اعمل Function عشان على حسب نوع الerror اللى اتبعت ارجع الresult 

            return HandleSingleProblem(errors[0]);


        }
        //this Handle Collection of Validation Errors
        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)//Means Validation Error اللى هى فى نص تنفيذ اى service يعنى اصلا الrequest اتبعت خلاص وبيتنفذ بس ظهر فى النص Validation Error 
                                                                                //انما فى validation Error تانى قصدى عليه دا وانا بدحل data غلط وانا ببعت الrequest ودا اصلا قبل مايبعت الrequest لاى حاجة ودا مش اللى اما بعمله دلؤقتى 
        {
            //هنا مبعوت اكتر من Validation Error جاية من services
            var modelstate = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelstate.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelstate);

        }


        #region Private Method Helper For Single Error مبعوتلها
        //this Method Handle Response على حسب نوع الerror اللى جايلها 
        private ActionResult HandleSingleProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeintoStatusCode(error.ErrorType)
                );
        }
        private static int MapErrorTypeintoStatusCode(ErrorType errorType)
       => errorType switch
       {

           ErrorType.NotFound => StatusCodes.Status404NotFound,
           ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
           ErrorType.Forbidden => StatusCodes.Status403Forbidden,
           ErrorType.Validation => StatusCodes.Status400BadRequest,
           ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
           _ => StatusCodes.Status500InternalServerError,
       };
        #endregion

    }
}
